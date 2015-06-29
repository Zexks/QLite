using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.Servers;
using QLite.Engines;
using QLite.Objects.Query;

namespace QLite.Controls
{
    public partial class QueryUI : UserControl
    {
        private Server _Svr;
        public BackgroundWorker bgwExecuteQuery = new BackgroundWorker();

        public Server Svr
        {
            get { return _Svr; }
            set { _Svr = value; }
        }

        public QueryUI()
        {
            _Svr = new Server();
            InitializeComponent();
        }

        public QueryUI(Server svr)
        {
            _Svr = svr;
            InitializeComponent();
        }

        public QueryUI(Server svr, string query)
        {
            _Svr = svr;
            InitializeComponent();
        }

        public void initializeServer(Server svr)
        {
            _Svr = new Server(svr);
            txtMessages.DataBindings.Add("Text", _Svr, "InfoMessages");
        }

        public void SetQuery(Query query)
        {
            string[] answers = new string[query.Variables.Count];
            for (int i = 0; i < query.Variables.Count; i++)
                if (QDialog.InputBox("Parameter " + (i + 1).ToString() + " of " + query.Variables.Count.ToString(), query.Variables[i].Question, ref answers[i]) == DialogResult.Cancel || answers[i] == "")
                    answers[i] = query.Variables[i].Default;
            txtQuery.Text = String.Format(query.Text, answers);
        }

        public void ExecuteQuery()
        {
            tpGrid.Controls.Clear();
            txtQuery.ReadOnly = true;
            //pbQueryBar.Visible = true;
            //pbQueryBar.MarqueeAnimationSpeed = 15;
            bgwExecuteQuery.RunWorkerAsync(txtQuery.Text);
        }

        private void txtQuery_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] file_path = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                StreamReader sReader = new StreamReader(file_path[0]);
                txtQuery.Text = sReader.ReadToEnd();
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "QueryPage", "txtQuery_DragDrop"); }
        }

        private void txtQuery_DragEnter(object sender, DragEventArgs e)
        {
            try
            { if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; }
            catch (Exception ex) { QMain.ReportEx(ex, "QueryPage", "txtQuery_DragEnter"); }
        }

        private void bgwExecuteQuery_DoWork(object sender, DoWorkEventArgs e)
        {
            QQuery.bgwQueryRunner = bgwExecuteQuery;
            e.Result = QQuery.ExecuteQuery(_Svr, (string)e.Argument);
        }

        private void bgwExecuteQuery_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //pbQueryBar.MarqueeAnimationSpeed = 0;
            //pbQueryBar.Visible = false;
            txtQuery.ReadOnly = false;

            if (!e.Cancelled)
            {
                DataSet tmpDataSet = (DataSet)e.Result;
                dgvOutput = new QGrid[tmpDataSet.Tables.Count];
                gridSplitters = new Splitter[tmpDataSet.Tables.Count];

                if (tmpDataSet.Tables.Count == 0)
                { /*pbQueryBar.MarqueeAnimationSpeed = 0;*/ txtMessages.Focus(); }
                else
                {
                    for (int i = 0; i < tmpDataSet.Tables.Count; i++)
                    {
                        //gridSplitters
                        gridSplitters[i] = new Splitter();
                        gridSplitters[i].Dock = DockStyle.Top;
                        gridSplitters[i].MinExtra = 0;

                        //dgvOutput
                        dgvOutput[i] = new QGrid();
                        dgvOutput[i].Name = "dgvOutput";
                        dgvOutput[i].Location = new System.Drawing.Point(5, 5);

                        if (tmpDataSet.Tables.Count > 1) dgvOutput[i].Dock = DockStyle.Top;
                        else dgvOutput[i].Dock = DockStyle.Fill;

                        dgvOutput[i].ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                        dgvOutput[i].SetDataTable(tmpDataSet.Tables[i]);
                        tpGrid.Controls.Add(dgvOutput[i]);

                        if (i + 1 != tmpDataSet.Tables.Count) tpGrid.Controls.Add(gridSplitters[i]);
                    }
                    /*if (dgvOutput.Length >= 1) lblDataCount.Text = "Row#: " + tmpDataSet.Tables[0].Rows.Count.ToString() + " | Col#: " + tmpDataSet.Tables[0].Columns.Count.ToString();
                    else lblDataCount.Text = "Tables returned: " + tmpDataSet.Tables.Count.ToString();*/
                }
            }
        }

        private void bgwExecuteQuery_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        
    }
}
