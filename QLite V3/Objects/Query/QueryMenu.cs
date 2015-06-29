using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLite.QueryPlugin;
using QLite.Controls;
using QLite.Collections;
using QLite.Engines;
using System.Xml;
using QLite.Events;
using QLite.Enumerations;

namespace QLite.Objects.Query
{
    public class QueryMenu : IDisposable
    {
        public event QuerySelected Query_Selected = delegate { };

        private QueryMenuItems _MenuItems;
        private ToolStripDropDownButton _MenuButton;
        private TreeNode _MenuNode;

        public event QuerySelected QuerySelected
        {
            add
            {
                if (Query_Selected != null)
                    lock (Query_Selected)
                        Query_Selected += value;
            }
            remove
            {
                if (Query_Selected != null)
                    lock (Query_Selected)
                        Query_Selected -= value;
            }
        }

        public QueryMenuItems MenuItems
        {
            get { return _MenuItems; }
        }

        public ToolStripDropDownButton MenuButton
        {
            get { return _MenuButton; }
        }

        public TreeNode MenuNode
        {
            get { return _MenuNode; }
        }

        public QueryMenu()
        { _MenuItems = new QueryMenuItems(); }

        public void Load()
        {
            XmlNode tmpMenuNode = QXML.PullMenus();
            foreach (XmlNode menuNode in tmpMenuNode.ChildNodes)
                if (menuNode.Name.ToUpper() == "QUERYMENU")
                    for (int i = 0; i < menuNode.ChildNodes.Count; i++)
                    {
                        QueryMenuItem tmpItem = CreateMenuItem(menuNode.ChildNodes[i]);
                        tmpItem.ID = i;
                        _MenuItems.Add(tmpItem);
                        _MenuItems[_MenuItems.IndexOf(tmpItem)].Load_QuerySet();
                    }
            AlignItems();
        }

        public void MoveQuery(int target, QueryMenuItems menus, Query query)
        {
            _MenuItems[_MenuItems.IndexOf(menus[0])].QuerySet.RemoveAt(menus[0].QuerySet.IndexOf(query));
            if (target >= _MenuItems[_MenuItems.IndexOf(menus[1])].QuerySet.Count - 1)
            { _MenuItems[_MenuItems.IndexOf(menus[1])].QuerySet.Add(query); }
            else { _MenuItems[_MenuItems.IndexOf(menus[1])].QuerySet.Insert(target, query); }
            _MenuItems[_MenuItems.IndexOf(menus[0])].Update_QuerySet();
            _MenuItems[_MenuItems.IndexOf(menus[1])].Update_QuerySet();
            _MenuItems.UpdateIndex();
            AlignControls();
        }

        public void MoveMenu(int target, QueryMenuItem menu)
        {
            int idx = _MenuItems.IndexOf(menu);
            _MenuItems.RemoveAt(idx);
            if (target >= _MenuItems.Count) { _MenuItems.Add(menu); }
            else { _MenuItems.Insert(target, menu); }
            idx = _MenuItems.IndexOf(menu);
            //foreach (Query query in _MenuItems[idx].QuerySet) _MenuItems[idx].QuerySet[_MenuItems[idx].QuerySet.IndexOf(query)].Parent = target;
            _MenuItems.UpdateIndex();
            AlignControls();
        }

        private void AlignItems()
        {
            int x = 0;
            QueryMenuItems tmpItems = new QueryMenuItems();
            for (int i = 0; i < _MenuItems.Count; i++)
                if (_MenuItems[i].Action != QAction.Delete)
                {
                    if (_MenuItems[i].Index == x)
                    { tmpItems.Add(_MenuItems[i]); x++; i = -1; }
                }
                else i = _MenuItems.Count;
            _MenuItems.Clear();
            _MenuItems = tmpItems;
            AlignControls();
        }

        private void AlignNodes()
        {
            _MenuNode = new TreeNode();

            foreach (QueryMenuItem item in _MenuItems)
                if (item.Action != QAction.Delete)
                {
                    TreeNode nodItem = new TreeNode();
                    nodItem.Name = "nodItem" + _MenuItems.IndexOf(item);
                    nodItem.Text = item.Name;
                    foreach (Query query in item.QuerySet)
                        if (query.Action != QAction.Delete)
                        {
                            TreeNode nodQuery = new TreeNode();
                            nodQuery.Name = "nodQuery" + item.QuerySet.IndexOf(query);
                            nodQuery.Text = query.Title;
                            nodItem.Nodes.Add(nodQuery);
                        }
                    _MenuNode.Nodes.Add(nodItem);
                }
        }

        public void AlignMenu()
        {
            _MenuButton = new ToolStripDropDownButton();

            foreach (QueryMenuItem item in _MenuItems)
            {
                ToolStripMenuItem btnMenu = new ToolStripMenuItem();
                btnMenu.Name = "btnMenu" + _MenuItems.IndexOf(item);
                btnMenu.Text = item.Name;
                btnMenu.Tag = _MenuItems.IndexOf(item);
                foreach (Query query in item.QuerySet)
                {
                    ToolStripMenuItem btnQuery = new ToolStripMenuItem();
                    btnQuery.Name = "btnQuery" + item.QuerySet.IndexOf(query);
                    btnQuery.Text = query.Title;
                    btnQuery.Tag = _MenuItems.IndexOf(item).ToString() + "," + item.QuerySet.IndexOf(query).ToString();
                    btnQuery.Click += new EventHandler(btnQuery_Click);
                    btnMenu.DropDownItems.Add(btnQuery);
                }
                _MenuButton.DropDownItems.Add(btnMenu);
            }
        }

        public void AlignControls()
        {
            AlignNodes();
            AlignMenu();
        }

        public QueryMenuItem CreateMenuItem(XmlNode menuItem)
        {
            QueryMenuItem item = new QueryMenuItem();
            foreach (XmlAttribute att in menuItem.Attributes)
                switch (att.Name.ToUpper())
                {
                    case "TITLE":
                        item.Name = att.Value.ToString();
                        break;
                    case "TYPE":
                        item.Index = Convert.ToInt32(att.Value);
                        break;
                }
            return item;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmpItem = (ToolStripMenuItem)sender;
            int menu = Convert.ToInt32(tmpItem.Tag.ToString().Substring(0, 1)), query = Convert.ToInt32(tmpItem.Tag.ToString().Substring(2, 1));
            Query_Selected(_MenuItems[menu].QuerySet[query], e);
        }

        #region Resource Cleanup

        private bool IsDisposed = false;

        public void Free()
        {
            if (IsDisposed)
            { throw new System.ObjectDisposedException("Object Name"); }
        }

        //Call Dispose to free resources explicitly
        public void Dispose()
        {
            //Pass true in dispose method to clean managed resources too and say GC to skip finalize in next line.
            Dispose(true);
            //If dispose is called already then say GC to skip finalize on this instance.
            GC.SuppressFinalize(this);

        }

        ~QueryMenu()
        {
            //Pass false as param because no need to free managed resources when you call finalize it will be done
            //by GC itself as its work of finalize to manage managed resources.
            Dispose(false);

        }

        //Implement dispose to free resources
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                // Released unmanaged Resources
                if (disposedStatus)
                {
                    // Released managed Resources
                }
            }
        }

        #endregion
    }

}
