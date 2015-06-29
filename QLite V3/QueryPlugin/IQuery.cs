using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using QLite.Servers;
using QLite.Controls;
using QLite.Engines;
using QLite.Enumerations;
using QLite.Objects.Query;
using QLite.Events;
using QLite.Plugins;

namespace QLite.IQuery
{
    [Export("IPlugin", typeof(IPlugin)),
     ExportMetadata("Name", "Query"),
     ExportMetadata("Author", "Jon Reed"),
     ExportMetadata("Version", 3)]

    class IQuery : IPlugin, IDisposable
    {
        public event PageUpdated _OnPageUpdated = delegate { };
        public event ExecuteQuery Execute_Query = delegate { };
        public event CancelQuery Cancel_Query = delegate { };
        public event DatabaseSelected Database_Selected = delegate { };

        public string Name { get; private set; }
        public string[] Info { get; private set; }
        public ToolStrip Menu { get; private set; }
        public UserControl UI { get; private set; }
        public Form Options { get; private set; }
        public Server Server { get; set; }
        public IPluginHost Host { get; private set; }

        public IQuery()
        {
            Name = "Query";
            Info = new string[] { "Jon Reed", "v.3", "Quick database analysis." };
            UI = new QueryUI();
            Options = new Form();
        }

        private void Load()
        {
            Server.LoadDatabaseSet();
            IQueryEngine.SetDatabases(tscbo_DatabaseSelector, Server.Databases);
            IQueryEngine.SetDatabase(tscbo_DatabaseSelector, Server.Connector.Database);
            BuildMenu();
        }

        private void BuildMenu()
        {
            tsdbtn_QueryMenu.DropDownItems.Clear();
            ToolStripMenuItem[] items = new ToolStripMenuItem[tsdbtn_QueryMenu.DropDownItems.Count];
            tsdbtn_QueryMenu.DropDownItems.CopyTo(items, 0);
            tsdbtn_QueryMenu.DropDownItems.AddRange(items);
        }

        public bool Initialize()
        {
            string svr = "";

            if (SetConnection(ref svr))
            {
                Server = new Server(svr); Load();
                UI = new QueryUI(Server) as UserControl;
                return true;
            }
            return false;
        }

        public bool Initialize(Server Server)
        {
            try
            {
                Server = new Server(Server); Load();
                UI = new QueryUI(Server) as UserControl;
                return true;
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "QueryUtility", "Initialize(Server)"); return false; }
        }

        public bool Initialize(Server Server, string query)
        {
            try
            {
                Server = new Server(Server); Load();
                UI = new QueryUI(Server, query) as UserControl;
                ExecuteQuery_Click(new Object(), new EventArgs());
                return true;
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "QueryUtility", "Initialize(Server, Query)"); return false; }
        }

        public void ExecuteQuery_Click(object sender, EventArgs e)
        {
            QueryUI tmpUI = UI as QueryUI;
            tmpUI.Svr.ConnectorDB = tscbo_DatabaseSelector.SelectedItem.ToString();
            tmpUI.Svr.initConnector();
            tmpUI.ExecuteQuery();
        }

        public void CancelQuery_Click(object sender, EventArgs e)
        {
            QueryUI tmpUI = UI as QueryUI;
            //tmpUI.bgwExecuteQuery.CancelAsync();
            tsbtn_Execute.Visible = true;
            tsbtn_Cancel.Visible = false;
        }

        private bool SetConnection(ref string svr)
        {
            while (QDialog.ServerLogonDialog(ref svr) != DialogResult.Cancel)
                if (svr != null)
                    if (QSQL.CheckConn(svr)) return true;
            return false;
        }

        public event PageUpdated OnPageUpdated
        {
            add
            {
                if (_OnPageUpdated != null)
                    lock (_OnPageUpdated)
                        _OnPageUpdated += value;
            }
            remove
            {
                if (_OnPageUpdated != null)
                    lock (_OnPageUpdated)
                        _OnPageUpdated -= value;
            }
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

        #region Resource Cleanup
        private bool IsDisposed = false;

        public void Free()
        {
            if (IsDisposed)
            { throw new System.ObjectDisposedException("Object Name"); }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~IQuery()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposedStatus) { }
            }
        }

        #endregion

        #region Controls
        private ToolStripButton tsbtn_Execute;
        private ToolStripButton tsbtn_Cancel;
        private ToolStripComboBox tscbo_DatabaseSelector;
        private ToolStripDropDownButton tsdbtn_QueryMenu;
        #endregion
    }

    class IQueryEngine
    {
        public static void SetDatabase(object sender, string value)
        {
            ToolStripComboBox tmp = (ToolStripComboBox)sender;
            foreach (string db in tmp.Items)
                    if (db == value) tmp.SelectedItem = db;
        }

        public static void SetDatabases(object sender, List<Database> dbs)
        {
            ToolStripComboBox tmp = (ToolStripComboBox)sender;
            foreach (Database db in dbs)
                tmp.Items.Add(db.Name);
        }

    }
}
