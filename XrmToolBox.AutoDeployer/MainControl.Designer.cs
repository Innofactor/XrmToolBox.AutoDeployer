namespace XrmToolBox.AutoDeployer
{
    partial class MainControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainControl));
            this.tsMenu = new System.Windows.Forms.ToolStrip();
            this.bAddPlugin = new System.Windows.Forms.ToolStripButton();
            this.ofdPlugin = new System.Windows.Forms.OpenFileDialog();
            this.listWatching = new System.Windows.Forms.ListView();
            this.file = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileupdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pluginupdated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tsMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMenu
            // 
            this.tsMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bAddPlugin,
            this.toolStripButton1});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(1061, 25);
            this.tsMenu.TabIndex = 0;
            this.tsMenu.Text = "toolStrip1";
            // 
            // bAddPlugin
            // 
            this.bAddPlugin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.bAddPlugin.Image = ((System.Drawing.Image)(resources.GetObject("bAddPlugin.Image")));
            this.bAddPlugin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bAddPlugin.Name = "bAddPlugin";
            this.bAddPlugin.Size = new System.Drawing.Size(121, 22);
            this.bAddPlugin.Text = "Add Plugin to Watch";
            this.bAddPlugin.Click += new System.EventHandler(this.bAddPlugin_Click);
            // 
            // ofdPlugin
            // 
            this.ofdPlugin.Filter = "Dataverse Plugin file|*.dll";
            this.ofdPlugin.Title = "Select Dataverse Plugin file";
            // 
            // listWatching
            // 
            this.listWatching.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.file,
            this.folder,
            this.fileupdated,
            this.pluginupdated,
            this.status});
            this.listWatching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listWatching.FullRowSelect = true;
            this.listWatching.HideSelection = false;
            this.listWatching.Location = new System.Drawing.Point(0, 0);
            this.listWatching.Name = "listWatching";
            this.listWatching.Size = new System.Drawing.Size(1061, 191);
            this.listWatching.TabIndex = 3;
            this.listWatching.UseCompatibleStateImageBehavior = false;
            this.listWatching.View = System.Windows.Forms.View.Details;
            this.listWatching.SelectedIndexChanged += new System.EventHandler(this.listWatching_SelectedIndexChanged);
            // 
            // file
            // 
            this.file.Text = "Plugin";
            this.file.Width = 141;
            // 
            // folder
            // 
            this.folder.Text = "Folder";
            this.folder.Width = 262;
            // 
            // fileupdated
            // 
            this.fileupdated.Text = "File Updated";
            this.fileupdated.Width = 124;
            // 
            // pluginupdated
            // 
            this.pluginupdated.Text = "Plugin Updated";
            this.pluginupdated.Width = 115;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 341;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(113, 22);
            this.toolStripButton1.Text = "Remove this Plugin";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listWatching);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Size = new System.Drawing.Size(1061, 331);
            this.splitContainer1.SplitterDistance = 191;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 4;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(1061, 132);
            this.txtLog.TabIndex = 0;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tsMenu);
            this.Name = "MainControl";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(1061, 356);
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMenu;
        private System.Windows.Forms.OpenFileDialog ofdPlugin;
        private System.Windows.Forms.ListView listWatching;
        private System.Windows.Forms.ToolStripButton bAddPlugin;
        private System.Windows.Forms.ColumnHeader file;
        private System.Windows.Forms.ColumnHeader folder;
        private System.Windows.Forms.ColumnHeader fileupdated;
        private System.Windows.Forms.ColumnHeader pluginupdated;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtLog;
    }
}
