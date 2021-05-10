namespace XrmToolBox.AutoDeployer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
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

        #region Internal Properties

        internal Dictionary<string, Resource> Information
        {
            get;
            private set;
        } = new Dictionary<string, Resource>();

        internal SemaphoreSlim Signal
        {
            get;
            private set;
        } = new SemaphoreSlim(1, 1);

        internal List<FileSystemWatcher> Watchers
        {
            get;
            private set;
        } = new List<FileSystemWatcher>();

        #endregion Internal Properties

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

        #region Private Methods

        private static bool IsAvailable(string location)
        {
            var counter = 0;

            // One minute in total operation to process the file: 120 retries with half a second delay between
            while (true && counter < 120)
            {
                try
                {
                    using (var stream = File.Open(location, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        if (stream != null)
                        {
                            return true;
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

                // Half a second delay before retry
                Task.Delay(500).Wait();
                counter++;
            }

            return false;
        }

        private void bFolder_Click(object sender, EventArgs e)
        {
            if (fbdFolder.ShowDialog() == DialogResult.OK)
            {
                ClearWatchers();
                SetWatchers(fbdFolder.SelectedPath);

                lFolder.Text = $"Watching folder '{fbdFolder.SelectedPath}'...";
            }
        }

        private void ClearWatchers()
        {
            if (Watchers != null)
            {
                foreach (var watcher in Watchers)
                {
                    watcher.Changed -= File_Changed;
                }
            }

            Watchers = new List<FileSystemWatcher>();
        }

        private void File_Changed(object sender, FileSystemEventArgs e)
        {
            var location = e.FullPath;

            if (!Information.ContainsKey(location))
            {
                Information.Add(location, new Resource(RetrieveMultiple, location));
            }

            var file = Information[location];

            if (file.Target == null)
            {
                return;
            }

            if (IsAvailable(location))
            {
                Invoke(new Action(() =>
                {
                    try
                    {
                        var lastWriteTime = File.GetLastWriteTime(location);
                        if (lastWriteTime > file.LastWriteTime)
                        {
                            tbLog.Text += $"{DateTime.Now}: File '{Path.GetFileName(location)}' was changed.\r\n";

                            var resource = new Entity(file.Target.LogicalName, file.Target.Id);
                            resource["content"] = Convert.ToBase64String(Resource.GetContents(location));

                            Update(resource);

                            tbLog.Text += $"{DateTime.Now}: File '{Path.GetFileName(location)}' was updated on the server.\r\n";

                            Information[location].LastWriteTime = lastWriteTime;
                        }
                    }
                    catch (Exception ex)
                    {
                        tbLog.Text += $"{DateTime.Now}: File '{Path.GetFileName(location)}' was not updated. The reason is exception raised: '{ex.Message}'.\r\n";
                    }
                }));
            }
        }

        private IEnumerable<Entity> RetrieveMultiple(QueryExpression query)
        {
            try
            {
                Signal.Wait();
                return Service?.RetrieveMultiple(query).Entities;
            }
            catch
            {
                return new List<Entity>();
            }
            finally
            {
                Signal.Release();
            }
        }

        private void SetWatchers(string location)
        {
            foreach (var patten in new string[] { "*.dll", "*.js", "*.json", "*.htm", "*.html", "*.css", "*.jpg", "*.jpeg", "*.gif", "*.png" })
            {
                var watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(location),
                    IncludeSubdirectories = true,
                    Filter = patten,

                    NotifyFilter = NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                watcher.Changed -= File_Changed;
                watcher.Changed += File_Changed;

                Watchers.Add(watcher);
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            // Preparing to dispose watcher
            if (Watchers != null)
            {
                ClearWatchers();
            }

            CloseTool();
        }

        private void Update(Entity item)
        {
            try
            {
                Signal.Wait();
                Service?.Update(item);
            }
            finally
            {
                Signal.Release();
            }
        }

        #endregion Private Methods
    }
}