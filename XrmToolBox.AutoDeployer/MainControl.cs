namespace XrmToolBox.AutoDeployer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using XrmToolBox.Extensibility;
    using XrmToolBox.Extensibility.Interfaces;

    public partial class MainControl : PluginControlBase, IGitHubPlugin, IWorkerHost, IAboutPlugin
    {
        #region Public Constructors

        public MainControl()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Properties

        string IGitHubPlugin.RepositoryName =>
            "XrmToolBox.AutoDeployer";

        string IGitHubPlugin.UserName =>
            "Innofactor";

        #endregion Public Properties

        #region Public Methods

        public void ShowAboutDialog()
        {
            try
            {
                var about = new About();
                //StartPosition = FormStartPosition.CenterParent
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion Public Methods

        #region Internal Properties

        internal DateTime LastRead
        {
            get;
            private set;
        }

        internal FileSystemWatcher Watcher
        {
            get;
            private set;
        }

        #endregion Internal Properties

        #region Private Methods

        private void bAddPlugin_Click(object sender, EventArgs e)
        {
            ofdPlugin.Filter = "MS CRM Plugins|*.dll";
            ofdPlugin.FileOk += (s, a) =>
            {
                var id = GetAssemblyId(ofdPlugin.FileName);

                if (id.Equals(Guid.Empty))
                {
                    MessageBox.Show("Please select valid MS Dynamics CRM plugin", "Incorrect file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                listWatching.Items.Clear();
                var plugin = new ListViewItem(new string[] {
                    Path.GetFileName(ofdPlugin.FileName),
                    Path.GetDirectoryName(ofdPlugin.FileName) ,
                    DateTime.Now.ToString("HH:mm:ss"),
                    string.Empty,
                    "Watching",
                    ofdPlugin.FileName,
                    id.ToString()
                });
                listWatching.Items.Add(plugin);

                Watcher = new FileSystemWatcher();
                Watcher.Path = Path.GetDirectoryName(ofdPlugin.FileName);
                Watcher.Filter = Path.GetFileName(ofdPlugin.FileName);

                Watcher.NotifyFilter = NotifyFilters.LastWrite;
                Watcher.EnableRaisingEvents = true;

                Watcher.Changed -= Plugin_Changed;
                Watcher.Changed += Plugin_Changed;
            };
            ofdPlugin.ShowDialog();
        }

        private Guid GetAssemblyId(string fileName)
        {
            var assembly = Assembly.Load(this.ReadFile(fileName));

            var chunks = assembly.FullName.Split(new string[] { ", ", "Version=", "Culture=", "PublicKeyToken=" }, StringSplitOptions.RemoveEmptyEntries);

            var query = new QueryExpression("pluginassembly");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, chunks[0]);
            query.Criteria.AddCondition("version", ConditionOperator.Equal, chunks[1]);
            query.Criteria.AddCondition("culture", ConditionOperator.Equal, chunks[2]);
            query.Criteria.AddCondition("publickeytoken", ConditionOperator.Equal, chunks[3]);

            var plugin = this.Service == null ? null : this.Service.RetrieveMultiple(query).Entities.FirstOrDefault();

            if (plugin != null)
            {
                return plugin.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        private void Plugin_Changed(object sender, FileSystemEventArgs e)
        {
            // Waiting for plugin become fully available for reading
            while (true)
            {
                try
                {
                    using (var stream = File.Open(e.FullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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

            this.Invoke(new Action(() =>
            {
                if (listWatching.Items.Count > 0)
                {
                    var file = e.FullPath;
                    try
                    {
                        var lastWriteTime = File.GetLastWriteTime(file);
                        if (lastWriteTime != LastRead)
                        {
                            listWatching.Items[0].SubItems[3].Text = DateTime.Now.ToString("HH:mm:ss.fff");
                            listWatching.Items[0].SubItems[4].Text = "Updating...";

                            var plugin = new Entity("pluginassembly", Guid.Parse(listWatching.Items[0].SubItems[6].Text));
                            plugin["content"] = Convert.ToBase64String(this.ReadFile(file));
                            Service.Update(plugin);

                            listWatching.Items[0].SubItems[3].Text = DateTime.Now.ToString("HH:mm:ss.fff");
                            listWatching.Items[0].SubItems[4].Text = "Update ok";
                            LastRead = lastWriteTime;
                        }
                    }
                    catch (Exception ex)
                    {
                        listWatching.Items[0].SubItems[4].Text = $"Error: {ex.Message}";
                    }
                }
            }));
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

        private void tsbClose_Click(object sender, EventArgs e)
        {
            // Preparing to dispose watcher
            if (this.Watcher != null)
            {
                this.Watcher.Changed -= this.Plugin_Changed;
            }

            this.CloseTool();
        }

        #endregion Private Methods
    }
}