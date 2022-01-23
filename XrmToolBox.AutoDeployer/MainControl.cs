namespace XrmToolBox.AutoDeployer
{
    using System;
    using System.IO;
    using System.Windows.Forms;
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

        #region Private/Internal Properties

        internal FileSystemWatcher Watcher
        {
            get;
            private set;
        }

        #endregion Private/Internal Properties

        #region Private Methods

        private void bAddPlugin_Click(object sender, EventArgs e)
        {
            if (ofdPlugin.ShowDialog()== DialogResult.OK)
            {
                listWatching.Items.Add(new WatchPluginFile(ofdPlugin.FileName, Service, this).ListItem);
            }
        }

        #endregion Private Methods

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listWatching.SelectedItems)
            {
                if (item.Tag is WatchPluginFile watch)
                {
                    watch.Dispose();
                }
                listWatching.Items.Remove(item);
            }
        }
    }
}