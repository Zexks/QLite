namespace QLite.Forms
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treInstances = new System.Windows.Forms.TreeView();
            this.tsConTools = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.miConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.miDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.miNewQuery = new System.Windows.Forms.ToolStripButton();
            this.qtabsMain = new QLite.Controls.QTabs2();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileManageQuerys = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileSaveDataSet = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUtilities = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpTopics = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tspMainStrip = new System.Windows.Forms.ToolStripPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tsConTools.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 24);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.treInstances);
            this.splitMain.Panel1.Controls.Add(this.tsConTools);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.qtabsMain);
            this.splitMain.Size = new System.Drawing.Size(618, 514);
            this.splitMain.SplitterDistance = 206;
            this.splitMain.TabIndex = 0;
            // 
            // treInstances
            // 
            this.treInstances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treInstances.Location = new System.Drawing.Point(0, 25);
            this.treInstances.Name = "treInstances";
            this.treInstances.Size = new System.Drawing.Size(206, 489);
            this.treInstances.TabIndex = 1;
            // 
            // tsConTools
            // 
            this.tsConTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.miNewQuery});
            this.tsConTools.Location = new System.Drawing.Point(0, 0);
            this.tsConTools.Name = "tsConTools";
            this.tsConTools.Size = new System.Drawing.Size(206, 25);
            this.tsConTools.TabIndex = 0;
            this.tsConTools.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miConnect,
            this.miDisconnect});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(68, 22);
            this.toolStripSplitButton1.Text = "Connect";
            // 
            // miConnect
            // 
            this.miConnect.Name = "miConnect";
            this.miConnect.Size = new System.Drawing.Size(133, 22);
            this.miConnect.Text = "Connect";
            // 
            // miDisconnect
            // 
            this.miDisconnect.Name = "miDisconnect";
            this.miDisconnect.Size = new System.Drawing.Size(133, 22);
            this.miDisconnect.Text = "Disconnect";
            // 
            // miNewQuery
            // 
            this.miNewQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.miNewQuery.Image = ((System.Drawing.Image)(resources.GetObject("miNewQuery.Image")));
            this.miNewQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miNewQuery.Name = "miNewQuery";
            this.miNewQuery.Size = new System.Drawing.Size(70, 22);
            this.miNewQuery.Tag = "Query";
            this.miNewQuery.Text = "New Query";
            this.miNewQuery.Click += new System.EventHandler(this.miNewQuery_Click);
            // 
            // qtabsMain
            // 
            this.qtabsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qtabsMain.Location = new System.Drawing.Point(0, 0);
            this.qtabsMain.Name = "qtabsMain";
            this.qtabsMain.SelectedIndex = 0;
            this.qtabsMain.Size = new System.Drawing.Size(408, 514);
            this.qtabsMain.TabIndex = 0;
            this.qtabsMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.qtabsMain_Selected);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuUtilities,
            this.mnuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(618, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileManageQuerys,
            this.miFileSaveDataSet,
            this.miFileClose});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // miFileManageQuerys
            // 
            this.miFileManageQuerys.Name = "miFileManageQuerys";
            this.miFileManageQuerys.Size = new System.Drawing.Size(160, 22);
            this.miFileManageQuerys.Text = "Manage Queries";
            // 
            // miFileSaveDataSet
            // 
            this.miFileSaveDataSet.Name = "miFileSaveDataSet";
            this.miFileSaveDataSet.Size = new System.Drawing.Size(160, 22);
            this.miFileSaveDataSet.Text = "Save Dataset";
            // 
            // miFileClose
            // 
            this.miFileClose.Name = "miFileClose";
            this.miFileClose.Size = new System.Drawing.Size(160, 22);
            this.miFileClose.Text = "Close";
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditOptions});
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "Edit";
            // 
            // miEditOptions
            // 
            this.miEditOptions.Name = "miEditOptions";
            this.miEditOptions.Size = new System.Drawing.Size(152, 22);
            this.miEditOptions.Text = "Options";
            this.miEditOptions.Click += new System.EventHandler(this.miEditOptions_Click);
            // 
            // mnuUtilities
            // 
            this.mnuUtilities.Name = "mnuUtilities";
            this.mnuUtilities.Size = new System.Drawing.Size(58, 20);
            this.mnuUtilities.Text = "Utilities";
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpTopics,
            this.miHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Help";
            // 
            // miHelpTopics
            // 
            this.miHelpTopics.Name = "miHelpTopics";
            this.miHelpTopics.Size = new System.Drawing.Size(109, 22);
            this.miHelpTopics.Text = "Topics";
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(109, 22);
            this.miHelpAbout.Text = "About";
            // 
            // tspMainStrip
            // 
            this.tspMainStrip.BackColor = System.Drawing.SystemColors.Control;
            this.tspMainStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.tspMainStrip.Location = new System.Drawing.Point(0, 24);
            this.tspMainStrip.Name = "tspMainStrip";
            this.tspMainStrip.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.tspMainStrip.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.tspMainStrip.Size = new System.Drawing.Size(618, 0);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 538);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.tspMainStrip);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "QLite";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tsConTools.ResumeLayout(false);
            this.tsConTools.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treInstances;
        private System.Windows.Forms.ToolStrip tsConTools;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuUtilities;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem miConnect;
        private System.Windows.Forms.ToolStripMenuItem miDisconnect;
        private System.Windows.Forms.ToolStripButton miNewQuery;
        private System.Windows.Forms.ToolStripMenuItem miFileManageQuerys;
        private System.Windows.Forms.ToolStripMenuItem miFileSaveDataSet;
        private System.Windows.Forms.ToolStripMenuItem miFileClose;
        private System.Windows.Forms.ToolStripMenuItem miEditOptions;
        private System.Windows.Forms.ToolStripMenuItem miHelpTopics;
        private System.Windows.Forms.ToolStripMenuItem miHelpAbout;
        private System.Windows.Forms.ToolStripPanel tspMainStrip;
        private Controls.QTabs2 qtabsMain;
    }
}

