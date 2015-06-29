using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.Servers;
using QLite.Events;

namespace QLite.Controls
{
    public class QMenuStrip : ToolStrip
    {
        public event ExecuteQuery Execute_Query = delegate { };
        public event CancelQuery Cancel_Query = delegate { };
        public event DatabaseSelected Database_Selected = delegate { };

        public QMenuStrip()
            : base()
        {
            initializeComponents();
        }

        public event ExecuteQuery ExecuteQuery
        {
            add
            {
                if (Execute_Query != null)
                    lock (Execute_Query)
                        Execute_Query += value;
            }
            remove
            {
                if (Execute_Query != null)
                    lock (Execute_Query)
                        Execute_Query -= value;
            }
        }

        public event CancelQuery CancelQuery
        {
            add
            {
                if (Cancel_Query != null)
                    lock (Cancel_Query)
                        Cancel_Query += value;
            }
            remove
            {
                if (Cancel_Query != null)
                    lock (Cancel_Query)
                        Cancel_Query -= value;
            }
        }

        public event DatabaseSelected DatabaseSelected
        {
            add
            {
                if (Database_Selected != null)
                    lock (Database_Selected)
                        Database_Selected += value;
            }
            remove
            {
                if (Database_Selected != null)
                    lock (Database_Selected)
                        Database_Selected -= value;
            }
        }

        public ToolStripButton btnExecute
        {
            get { return tsbtn_ExecuteQuery; }
        }

        public ToolStripButton btnCancel
        {
            get { return tsbtn_CancelQuery; }
        }

        public ToolStripDropDownButton btnQueryMenu
        {
            get { return tsdbtn_QueryMenu; }
            set { tsdbtn_QueryMenu = value; }
        }

        public string GetDatabase
        {
            get { return tscbo_DatabaseSelector.SelectedItem.ToString(); }
        }

        public string SetDatabase
        {
            set
            {
                foreach (string db in tscbo_DatabaseSelector.Items)
                    if (db == value) tscbo_DatabaseSelector.SelectedItem = db;
            }
        }

        public void initializeComponents()
        {
            tsbtn_ExecuteQuery = new ToolStripButton();
            tsbtn_CancelQuery = new ToolStripButton();
            tscbo_DatabaseSelector = new ToolStripComboBox();
            tsdbtn_QueryMenu = new ToolStripDropDownButton();

            //tsbtn_ExecuteQuery
            tsbtn_ExecuteQuery.Name = "tsbtn_ExecuteQuery";
            tsbtn_ExecuteQuery.Text = "E&xecute";
            tsbtn_ExecuteQuery.Click += new EventHandler(tsbtn_ExecuteQuery_Click);

            //tsbtn_CancelQuery
            tsbtn_CancelQuery.Name = "tsbtn_CancelQuery";
            tsbtn_CancelQuery.Text = "&Cancel";
            tsbtn_CancelQuery.Visible = false;
            tsbtn_CancelQuery.Click += new EventHandler(tsbtn_CancelQuery_Click);

            //tscbo_DatabaseSelector
            tscbo_DatabaseSelector.Name = "tscbo_DatabaseSelector";
            tscbo_DatabaseSelector.SelectedIndexChanged += new EventHandler(tscbo_DatabaseSelector_SelectedIndexChanged);

            //ts_ddbQueries
            tsdbtn_QueryMenu.Name = "tsdbtn_QueryMenu";
            tsdbtn_QueryMenu.Text = "Queries";

            this.Items.AddRange(new ToolStripItem[] { tsbtn_ExecuteQuery, tsbtn_CancelQuery, tscbo_DatabaseSelector, tsdbtn_QueryMenu });

        }

        private void tsbtn_ExecuteQuery_Click(object sender, EventArgs e)
        {
            Execute_Query(sender, e);
        }

        private void tsbtn_CancelQuery_Click(object sender, EventArgs e)
        {
            Cancel_Query(sender, e);
        }

        private void tscbo_DatabaseSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Database_Selected(sender, e);
        }

        public void SetDatabases(List<Database> dbs)
        {
            foreach (Database db in dbs)
                tscbo_DatabaseSelector.Items.Add(db.Name);
        }

        #region Controls
        private ToolStripButton tsbtn_ExecuteQuery;
        private ToolStripButton tsbtn_CancelQuery;
        private ToolStripComboBox tscbo_DatabaseSelector;
        private ToolStripDropDownButton tsdbtn_QueryMenu;
        #endregion

    }

}
