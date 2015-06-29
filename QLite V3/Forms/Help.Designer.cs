namespace QLite.Forms
{
    partial class Help
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
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treTopics = new System.Windows.Forms.TreeView();
            this.txtInfo = new System.Windows.Forms.TextBox();

            //treTopics
            this.treTopics.Name = "treTopics";
            this.treTopics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treTopics.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treTopics_AfterSelect);

            //txtInfo
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInfo.TabIndex = 0;
            this.txtInfo.Multiline = true;
            this.txtInfo.ReadOnly = true;

            //splitMain
            this.splitMain.Name = "splitMain";
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Panel1.Controls.Add(this.treTopics);
            this.splitMain.Panel2.Controls.Add(this.txtInfo);

            //HelpDialog
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 425);
            this.MinimumSize = this.ClientSize;
            this.Controls.Add(splitMain);
            this.Name = "HelpDialog";
            this.Text = "QLite Help";
            this.Load += new System.EventHandler(Form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        #region Controls
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treTopics;
        private System.Windows.Forms.TextBox txtInfo;

        #endregion

    }
}