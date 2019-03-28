namespace Innofactor.XTB.AutoDeployer
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

    public partial class MainControl : PluginControlBase, IGitHubPlugin, IWorkerHost
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

        #region Internal Properties

        internal DateTime LastRead
        {
            get;
            private set;
        }

        internal Guid PluginId
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

        private void bPlugin_Click(object sender, EventArgs e)
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

                    PluginId = id;

                    lPlugin.Text = ofdPlugin.FileName;

                    Watcher = new FileSystemWatcher
                    {
                        Path = Path.GetDirectoryName(lPlugin.Text),
                        Filter = Path.GetFileName(lPlugin.Text),

                        NotifyFilter = NotifyFilters.LastWrite,
                        EnableRaisingEvents = true
                    };

                    Watcher.Changed -= Plugin_Changed;
                    Watcher.Changed += Plugin_Changed;
                };
            ofdPlugin.ShowDialog();
        }

        private Guid GetAssemblyId(string fileName)
        {
            var assembly = Assembly.Load(ReadFile(fileName));

            var chunks = assembly.FullName.Split(new string[] { ", ", "Version=", "Culture=", "PublicKeyToken=" }, StringSplitOptions.RemoveEmptyEntries);

            var query = new QueryExpression("pluginassembly");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, chunks[0]);
            query.Criteria.AddCondition("version", ConditionOperator.Equal, chunks[1]);
            query.Criteria.AddCondition("culture", ConditionOperator.Equal, chunks[2]);
            query.Criteria.AddCondition("publickeytoken", ConditionOperator.Equal, chunks[3]);

            var plugin = Service?.RetrieveMultiple(query).Entities.FirstOrDefault();

            return (plugin != null)
                ? plugin.Id
                : Guid.Empty;
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

            Invoke(new Action(() =>
                {
                    try
                    {
                        var lastWriteTime = File.GetLastWriteTime(lPlugin.Text);
                        if (lastWriteTime != LastRead)
                        {
                            tbLog.Text += string.Format("{0}: Assembly '{1}' was changed.\r\n", DateTime.Now, Path.GetFileName(lPlugin.Text));

                            var plugin = new Entity("pluginassembly")
                            {
                                Id = PluginId
                            };

                            plugin["content"] = Convert.ToBase64String(ReadFile(lPlugin.Text));

                            Service.Update(plugin);

                            tbLog.Text += string.Format("{0}: Assembly '{1}' was updated on the server.\r\n", DateTime.Now, Path.GetFileName(lPlugin.Text));

                            LastRead = lastWriteTime;
                        }
                    }
                    catch (Exception ex)
                    {
                        tbLog.Text += string.Format("{0}: Assembly '{1}' was not updated. The reason is exception raised: '{2}'.\r\n", DateTime.Now, Path.GetFileName(lPlugin.Text), ex.Message);
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
            if (Watcher != null)
            {
                Watcher.Changed -= Plugin_Changed;
            }

            CloseTool();
        }

        #endregion Private Methods
    }
}