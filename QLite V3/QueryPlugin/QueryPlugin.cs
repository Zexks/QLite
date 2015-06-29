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

namespace QLite.QueryPlugin
{
    public class QueryPlug :  IDisposable
    {
        private QueryMenu MainMenu;
        public UserControl UI { get; private set; }
        public QMenuStrip Menu { get; private set; }
        public Server Svr { get; set; }

        public QueryPlug()
        {
            Menu = new QMenuStrip();
            MainMenu = new QueryMenu(); MainMenu.Load();
            Menu.ExecuteQuery += new ExecuteQuery(ExecuteQuery_Click);
            Menu.CancelQuery += new CancelQuery(CancelQuery_Click);
            Menu.DatabaseSelected += new DatabaseSelected(MenuStrip_DatabaseSelected);
            MainMenu.QuerySelected += new QuerySelected(MenuStrip_QuerySelected);
        }

        private void Load()
        {
            Svr.LoadDatabaseSet();
            Menu.SetDatabases(Svr.Databases);
            Menu.SetDatabase = Svr.Connector.Database;
            BuildMenu();
        }

        private void BuildMenu()
        {
            Menu.btnQueryMenu.DropDownItems.Clear();
            ToolStripMenuItem[] items = new ToolStripMenuItem[MainMenu.MenuButton.DropDownItems.Count];
            MainMenu.MenuButton.DropDownItems.CopyTo(items, 0);
            Menu.btnQueryMenu.DropDownItems.AddRange(items);
        }

        public bool Initialize()
        {
            string svr = "";

            if (SetConnection(ref svr))
            {
                Svr = new Server(svr); Load();
                UI = new QueryUI(Svr) as UserControl;
                return true;
            }
            return false;
        }

        public bool Initialize(Server svr)
        {
            try
            {
                Svr = new Server(svr); Load();
                UI = new QueryUI(Svr) as UserControl;
                return true;
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "QueryUtility", "Initialize(Server)"); return false; }
        }

        public bool Initialize(Server svr, string query)
        {
            try
            {
                Svr = new Server(svr); Load();
                UI = new QueryUI(svr, query) as UserControl;
                ExecuteQuery_Click(new Object(), new EventArgs());
                return true;
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "QueryUtility", "Initialize(Server, Query)"); return false; }
        }

        public void ExecuteQuery_Click(object sender, EventArgs e)
        {
            QueryUI tmpUI = UI as QueryUI;
            tmpUI.Svr.ConnectorDB = Menu.GetDatabase;
            tmpUI.Svr.initConnector();
            tmpUI.ExecuteQuery();
        }

        public void CancelQuery_Click(object sender, EventArgs e)
        {
            QueryUI tmpUI = UI as QueryUI;
            //tmpUI.bgwExecuteQuery.CancelAsync();
            Menu.btnExecute.Visible = true;
            Menu.btnCancel.Visible = false;
        }

        private bool SetConnection(ref string svr)
        {
            while (QDialog.ServerLogonDialog(ref svr) != DialogResult.Cancel)
                if (svr != null)
                    if (QSQL.CheckConn(svr)) return true;
            return false;
        }

        private void MenuStrip_DatabaseSelected(object sender, EventArgs e)
        {
            Svr.Connector.Open();
            Svr.Connector.ChangeDatabase(Menu.GetDatabase);
            Svr.Connector.Close();
        }

        private void MenuStrip_QuerySelected(object sender, EventArgs e)
        {
            Query tmpQuery = (Query)sender;
            QueryUI tmpUI = (QueryUI)UI;
            //tmpUI.SetQuery(tmpQuery);
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

        ~QueryPlug()
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
    }

}
