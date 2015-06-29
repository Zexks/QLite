namespace QLite.Controls
{
    partial class QueryUI
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
            this.spltMain = new System.Windows.Forms.SplitContainer();
            this.pnlQuery = new System.Windows.Forms.Panel();
            this.txtQuery = new QLite.Controls.QText();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGrid = new System.Windows.Forms.TabPage();
            this.tpText = new System.Windows.Forms.TabPage();
            this.txtMessages = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).BeginInit();
            this.spltMain.Panel1.SuspendLayout();
            this.spltMain.Panel2.SuspendLayout();
            this.spltMain.SuspendLayout();
            this.pnlQuery.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpText.SuspendLayout();
            this.SuspendLayout();
            // 
            // spltMain
            // 
            this.spltMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMain.Location = new System.Drawing.Point(0, 0);
            this.spltMain.Name = "spltMain";
            this.spltMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltMain.Panel1
            // 
            this.spltMain.Panel1.Controls.Add(this.pnlQuery);
            this.spltMain.Panel1.Padding = new System.Windows.Forms.Padding(0, 5, 5, 0);
            // 
            // spltMain.Panel2
            // 
            this.spltMain.Panel2.Controls.Add(this.tabControl1);
            this.spltMain.Size = new System.Drawing.Size(299, 306);
            this.spltMain.SplitterDistance = 136;
            this.spltMain.TabIndex = 0;
            // 
            // pnlQuery
            // 
            this.pnlQuery.Controls.Add(this.txtQuery);
            this.pnlQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQuery.Location = new System.Drawing.Point(0, 5);
            this.pnlQuery.Name = "pnlQuery";
            this.pnlQuery.Size = new System.Drawing.Size(294, 131);
            this.pnlQuery.TabIndex = 1;
            // 
            // txtQuery
            // 
            this.txtQuery.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this.txtQuery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtQuery.CommentPrefix = "--";
            this.txtQuery.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.LeftBracket = '(';
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.RightBracket = ')';
            this.txtQuery.Size = new System.Drawing.Size(294, 131);
            this.txtQuery.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGrid);
            this.tabControl1.Controls.Add(this.tpText);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(299, 166);
            this.tabControl1.TabIndex = 0;
            // 
            // tpGrid
            // 
            this.tpGrid.Location = new System.Drawing.Point(4, 22);
            this.tpGrid.Name = "tpGrid";
            this.tpGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tpGrid.Size = new System.Drawing.Size(291, 140);
            this.tpGrid.TabIndex = 0;
            this.tpGrid.Text = "Results";
            this.tpGrid.UseVisualStyleBackColor = true;
            // 
            // tpText
            // 
            this.tpText.Controls.Add(this.txtMessages);
            this.tpText.Location = new System.Drawing.Point(4, 22);
            this.tpText.Name = "tpText";
            this.tpText.Padding = new System.Windows.Forms.Padding(3);
            this.tpText.Size = new System.Drawing.Size(291, 140);
            this.tpText.TabIndex = 1;
            this.tpText.Text = "Messages";
            this.tpText.UseVisualStyleBackColor = true;
            // 
            // txtMessages
            // 
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(3, 3);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(285, 134);
            this.txtMessages.TabIndex = 0;
            this.txtMessages.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 284);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(299, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // QueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.spltMain);
            this.DoubleBuffered = true;
            this.Name = "QueryUI";
            this.Size = new System.Drawing.Size(299, 306);
            this.spltMain.Panel1.ResumeLayout(false);
            this.spltMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMain)).EndInit();
            this.spltMain.ResumeLayout(false);
            this.pnlQuery.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer spltMain;
        private QText txtQuery;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGrid;
        private System.Windows.Forms.TabPage tpText;
        private System.Windows.Forms.RichTextBox txtMessages;
        private System.Windows.Forms.Splitter[] gridSplitters;
        private QGrid[] dgvOutput;
        private System.Windows.Forms.Panel pnlQuery;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}
