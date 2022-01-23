using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace XrmToolBox.AutoDeployer
{
    internal class WatchPluginFile : IDisposable
    {
        private Control owner;
        private IOrganizationService service;

        public ListViewItem ListItem { get; private set; }
        public string File { get; private set; }
        public string Path { get; private set; }
        public DateTime FileUpdated { get; set; }
        public DateTime PluginUpdated { get; set; }
        public string Status { get; private set; }
        public string FullPath { get; private set; }
        public Guid PluginId { get; private set; }
        public string Log { get; set; }
        public FileSystemWatcher Watcher { get; }

        public event EventHandler Changed;

        protected virtual void OnChanged() { Changed?.Invoke(this, EventArgs.Empty); }

        public WatchPluginFile(string filename, IOrganizationService Service, Control Owner)
        {
            owner = Owner;
            service = Service;
            FullPath = filename;
            File = System.IO.Path.GetFileName(filename);
            Path = System.IO.Path.GetDirectoryName(filename);
            Status = "Watching";
            Log = $"Started at {DateTime.Now}\r\n";
            PluginId = GetAssemblyId(Service);
            if (PluginId != Guid.Empty)
            {
                Watcher = new FileSystemWatcher();
                Watcher.Path = Path;
                Watcher.Filter = File;
                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.EnableRaisingEvents = true;
                Watcher.Changed += Plugin_Changed;
            }
            ListItem = new ListViewItem();
            ListItem.Tag = this;
            UpdateList();
        }

        private void Plugin_Changed(object sender, FileSystemEventArgs e)
        {
            // Waiting for plugin become fully available for reading
            while (true)
            {
                try
                {
                    using (var stream = System.IO.File.Open(e.FullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        if (stream != null)
                        {
                            break;
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }

                Thread.Sleep(500);
            }

            var file = e.FullPath;
            try
            {
                var lastWriteTime = System.IO.File.GetLastWriteTime(file);
                if (lastWriteTime != FileUpdated)
                {
                    FileUpdated = lastWriteTime;
                    Status = "Updating...";
                    Log += DateTime.Now.ToString("HH:mm:ss.fff") + $" File updated\r\n";
                    UpdateList();

                    var plugin = new Entity("pluginassembly", PluginId);
                    plugin["content"] = Convert.ToBase64String(ReadFile(file));
                    service.Update(plugin);

                    PluginUpdated = DateTime.Now;
                    Status = "Update ok";
                    Log += DateTime.Now.ToString("HH:mm:ss.fff") + $" Dataverse plugin updated\r\n";
                    UpdateList();
                }
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
                UpdateList();
            }
        }

        private void UpdateList()
        {
            MethodInvoker mi = delegate
            {
                while (ListItem.SubItems.Count < 5)
                {
                    ListItem.SubItems.Add(string.Empty);
                }
                ListItem.Text = File;
                ListItem.SubItems[1].Text = Path;
                ListItem.SubItems[2].Text = FileUpdated.Ticks != 0 ? FileUpdated.ToString("HH.mm:ss.fff") : string.Empty;
                ListItem.SubItems[3].Text = PluginUpdated.ToString("HH:mm:ss.fff");
                ListItem.SubItems[4].Text = Status;
                OnChanged();
            };
            if (owner.InvokeRequired)
            {
                owner.Invoke(mi);
            }
            else
            {
                mi();
            }
        }

        private Guid GetAssemblyId(IOrganizationService Service)
        {
            if (Service == null)
            {
                return Guid.Empty;
            }
            var assembly = Assembly.Load(ReadFile(FullPath));
            var chunks = assembly.FullName.Split(new string[] { ", ", "Version=", "Culture=", "PublicKeyToken=" }, StringSplitOptions.RemoveEmptyEntries);
            var query = new QueryExpression("pluginassembly");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, chunks[0]);
            query.Criteria.AddCondition("version", ConditionOperator.Equal, chunks[1]);
            query.Criteria.AddCondition("culture", ConditionOperator.Equal, chunks[2]);
            query.Criteria.AddCondition("publickeytoken", ConditionOperator.Equal, chunks[3]);
            return Service.RetrieveMultiple(query).Entities.FirstOrDefault()?.Id ?? Guid.Empty;
        }

        private byte[] ReadFile(string fileName)
        {
            byte[] buffer = null;
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        public void Dispose()
        {
            Watcher.Changed -= Plugin_Changed;
        }
    }
}
