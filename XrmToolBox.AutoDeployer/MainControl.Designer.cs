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
            this.ofdPlugin = new System.Windows.Forms.OpenFileDialog();
            this.listWatching = new System.Windows.Forms.ListView();
            this.bAddPlugin = new System.Windows.Forms.ToolStripButton();
            this.folder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.file = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.addtime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.updated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.log = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMenu
            // 
            this.tsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bAddPlugin});
            this.tsMenu.Location = new System.Drawing.Point(0, 0);
            this.tsMenu.Name = "tsMenu";
            this.tsMenu.Size = new System.Drawing.Size(1061, 25);
            this.tsMenu.TabIndex = 0;
            this.tsMenu.Text = "toolStrip1";
            // 
            // listWatching
            // 
            this.listWatching.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.file,
            this.folder,
            this.addtime,
            this.updated,
            this.log});
            this.listWatching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listWatching.HideSelection = false;
            this.listWatching.Location = new System.Drawing.Point(0, 25);
            this.listWatching.Name = "listWatching";
            this.listWatching.Size = new System.Drawing.Size(1061, 331);
            this.listWatching.TabIndex = 3;
            this.listWatching.UseCompatibleStateImageBehavior = false;
            this.listWatching.View = System.Windows.Forms.View.Details;
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
            // folder
            // 
            this.folder.Text = "Folder";
            this.folder.Width = 262;
            // 
            // file
            // 
            this.file.Text = "Plugin";
            this.file.Width = 141;
            // 
            // addtime
            // 
            this.addtime.Text = "Added";
            this.addtime.Width = 124;
            // 
            // updated
            // 
            this.updated.Text = "Updated";
            this.updated.Width = 115;
            // 
            // log
            // 
            this.log.Text = "Log";
            this.log.Width = 341;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.listWatching);
            this.Controls.Add(this.tsMenu);
            this.Name = "MainControl";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(1061, 356);
            this.tsMenu.ResumeLayout(false);
            this.tsMenu.PerformLayout();
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
        private System.Windows.Forms.ColumnHeader addtime;
        private System.Windows.Forms.ColumnHeader updated;
        private System.Windows.Forms.ColumnHeader log;
    }
}
