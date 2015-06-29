using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using QLite.Plugins;
using QLite.Objects.Query;

namespace QLite.Engines
{
    public class QEngine_XML : IDisposable
    {
        #region Counts
        public static int CountOneDeep(string item1)
        {
            int count = 0;
            Global.QliteDefaults.XmlDefaults.Load("QLite2.xml");

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == item1)
                        count++;

            return count;

        }

        public static int CountTwoDeep(string item1, string item2)
        {
            int count = 0;
            Global.QliteDefaults.XmlDefaults.Load("QLite2.xml");

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == item1)
                        foreach (XmlNode node3 in node2.ChildNodes)
                            if (node3.Name == item2)
                                count++;

            return count;
        }

        public static int CountThreeDeep(string item1, string item2, string item3)
        {
            int count = 0;

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name.ToUpper() == item1.ToUpper())
                        foreach (XmlNode node3 in node2.ChildNodes)
                            if (node3.Name.ToUpper() == item2.ToUpper())
                                foreach (XmlNode node4 in node3.ChildNodes)
                                    if (node4.Name.ToUpper() == item3.ToUpper())
                                        count++;

            return count;
        }

        #endregion

        #region Pull Nodes

        public static XmlNode PullOneDeep(string item1)
        {
            XmlNode tmpNode = Global.QliteDefaults.XmlDefaults.Clone();

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == item1)
                    { tmpNode = node2.Clone(); break; }

            return tmpNode;
        }

        public static XmlNode PullTwoDeep(string item1, string item2)
        {
            XmlNode tmpNode = Global.QliteDefaults.XmlDefaults.Clone();

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == item1)
                    {
                        foreach (XmlNode node3 in node2.ChildNodes)
                            if (node3.Name == item2)
                            { tmpNode = node3.Clone(); break; }
                        break;
                    }

            return tmpNode;
        }

        public static XmlNode PullThreeDeep(string item1, string item2, string item3)
        {
            XmlNode tmpNode = Global.QliteDefaults.XmlDefaults.Clone();

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes)
                foreach (XmlNode node2 in node1.ChildNodes)
                    if (node2.Name == item1)
                    {
                        foreach (XmlNode node3 in node2.ChildNodes)
                            if (node3.Name == item2)
                            {
                                foreach (XmlNode node4 in node3.ChildNodes)
                                    if (node4.Name == item3)
                                    { tmpNode = node4.Clone(); break; }
                                break;
                            }
                        break;
                    }

            return tmpNode;
        }

        #endregion

        #region Options
        public static XmlNode PullDefault(string set, string def)
        {
            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node1.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name == set)
                            foreach (XmlNode node3 in node2.ChildNodes)
                                if (node3.Name == def)
                                    return node3;

            return null;
        }

        public static void UpdateOption(string rootNode, string node, string[] values)
        {
            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0].ChildNodes)
                if (node1.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name == rootNode)
                            foreach (XmlNode node3 in node2.ChildNodes)
                                if (node3.Name == node)
                                {
                                    XmlAttributeCollection attCollect = node3.Attributes;
                                    for (int i = 0; i < values.Length; i++)
                                        node3.Attributes[i].InnerText = values[i];
                                }

            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        /*public static void UpdateFont(QliteFont qFont)
        {
            foreach(XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0].ChildNodes)
                if(node1.Name.ToUpper() == "OPTIONS")
                    foreach(XmlNode node2 in node1.ChildNodes)
                        if(node2.Name.ToUpper() == "DEFAULTS")
                            foreach(XmlNode node3 in node2.ChildNodes)
                                if (node3.Name.ToUpper() == "FONTS")
                                    foreach (XmlNode fontNode in node3.ChildNodes)
                                        if (fontNode.Name.ToUpper() == qFont.Type.ToString().ToUpper())
                                        { node3.ReplaceChild(CreateFontNode(qFont), fontNode); goto Finish; }
            Finish:
                using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }
        */
        /*private static XmlNode CreateFontNode(QliteFont qFont)
        {
            XmlNode newQueryNode = Global.QliteDefaults.XmlDefaults.CreateNode(XmlNodeType.Element, qFont.Type.ToString(), Global.QliteDefaults.XmlDefaults.NamespaceURI);
            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Name"));
            newQueryNode.Attributes[0].Value = qFont.QFont.Name;

            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Size"));
            newQueryNode.Attributes[1].Value = qFont.QFont.Size.ToString();

            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Style"));
            foreach (bool s in qFont.Style) newQueryNode.Attributes[2].Value += Convert.ToInt32(s).ToString();

            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("fColor"));
            newQueryNode.Attributes[3].Value = qFont.Colors.fColor.A.ToString() + "," +
                                               qFont.Colors.fColor.R.ToString() + "," +
                                               qFont.Colors.fColor.G.ToString() + "," +
                                               qFont.Colors.fColor.B.ToString();
            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("bColor"));
            newQueryNode.Attributes[4].Value = qFont.Colors.bColor.A.ToString() + "," +
                                               qFont.Colors.bColor.R.ToString() + "," +
                                               qFont.Colors.bColor.G.ToString() + "," +
                                               qFont.Colors.bColor.B.ToString();
            return newQueryNode;
        }
        */
        #endregion

        #region Plugins
        public static XmlNode PullPluginOptions(IPlugin inPlug)
        {
            foreach (XmlNode optionNode in Global.QliteDefaults.XmlDefaults.ChildNodes[0].ChildNodes)
                if (optionNode.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode pluginsNode in optionNode.ChildNodes)
                        if (pluginsNode.Name.ToUpper() == "PLUGINS")
                            foreach (XmlNode plugNode in pluginsNode.ChildNodes)
                                if (plugNode.Name.ToUpper() == inPlug.ToString().ToUpper())
                                    return plugNode;

            return null;
        }

        public static bool PluginOptionsExist(IPlugin inPlug)
        {
            foreach (XmlNode optionsNode in Global.QliteDefaults.XmlDefaults.ChildNodes[0].ChildNodes)
                if (optionsNode.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode node2 in optionsNode.ChildNodes)
                        if (node2.Name.ToUpper() == "PLUGINS")
                            foreach (XmlNode node3 in node2.ChildNodes)
                                if (node3.Name.ToUpper() == inPlug.ToString().ToUpper())
                                    return true;
            return false;
        }

        public static bool AddPluginNode(XmlNode plugNode)
        {
            bool complete = false;

            foreach (XmlNode optionsNode in Global.QliteDefaults.XmlDefaults.ChildNodes[0].ChildNodes)
                if (optionsNode.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode pluginsNode in optionsNode.ChildNodes)
                        if (pluginsNode.Name.ToUpper() == "PLUGINS")
                        { pluginsNode.AppendChild(plugNode); complete = true; goto Finish; }

        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }

            return complete;
        }

        public static void UpdatePluginNode(XmlNode node)
        {
            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node1.Name.ToUpper() == "OPTIONS")
                    foreach (XmlNode node2 in node1.ChildNodes)
                        if (node2.Name.ToUpper() == "PLUGINS")
                            foreach (XmlNode node3 in node2.ChildNodes)
                                if (node3.Name == node.Name)
                                { node2.ReplaceChild(node, node3); goto Finish; }

        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        #endregion

        #region Queries
        public static XmlNode PullQueries()
        {
            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node1.Name.ToUpper() == "QUERIES")
                    return node1;
            return null;
        }

        private static XmlNode CreateQueryNode(Query inQuery)
        {
            XmlNode newQueryNode = Global.QliteDefaults.XmlDefaults.CreateNode(XmlNodeType.Element, "Query", Global.QliteDefaults.XmlDefaults.NamespaceURI);
            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Title"));
            newQueryNode.Attributes[0].Value = inQuery.Title;
            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Type"));
            newQueryNode.Attributes[1].Value = inQuery.Parent.ToString();
            newQueryNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Index"));
            newQueryNode.Attributes[2].Value = inQuery.Index.ToString();
            newQueryNode.AppendChild(Global.QliteDefaults.XmlDefaults.CreateCDataSection(inQuery.Text));
            foreach (QueryVariable var in inQuery.Variables)
            {
                XmlNode varNode = Global.QliteDefaults.XmlDefaults.CreateNode(XmlNodeType.Element, "Var", Global.QliteDefaults.XmlDefaults.NamespaceURI);
                varNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Default"));
                varNode.Attributes[0].Value = inQuery.Variables[inQuery.Variables.IndexOf(var)].Default;
                varNode.AppendChild(Global.QliteDefaults.XmlDefaults.CreateCDataSection(inQuery.Variables[inQuery.Variables.IndexOf(var)].Question));
                newQueryNode.AppendChild(varNode);
            }
            return newQueryNode;
        }

        public static void AddQuery(Query query)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "QUERIES")
                { node.AppendChild(CreateQueryNode(query)); break; }

            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        public static void UpdateQuery(Query newQuery, Query oldQuery)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "QUERIES")
                    foreach (XmlNode queryNode in node.ChildNodes)
                        foreach (XmlAttribute att in queryNode.Attributes)
                            if (att.Name.ToUpper() == "TITLE" && att.Value == oldQuery.Title)
                            { node.ReplaceChild(CreateQueryNode(newQuery), queryNode); goto Finish; }
        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        public static void DeleteQuery(Query query)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "QUERIES")
                    foreach (XmlNode queryNode in node.ChildNodes)
                        foreach (XmlAttribute att in queryNode.Attributes)
                            if (att.Name.ToUpper() == "TITLE")
                                if (att.Value.ToUpper() == query.Title.ToUpper())
                                { node.RemoveChild(queryNode); goto Finish; }
        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        #endregion

        #region Menus
        public static XmlNode PullMenus()
        {
            foreach (XmlNode menus in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (menus.Name.ToUpper() == "MENUS")
                    return menus;
            return null;
        }

        private static XmlNode CreateMenuNode(QueryMenuItem menu)
        {
            XmlNode newMenuNode = Global.QliteDefaults.XmlDefaults.CreateNode(XmlNodeType.Element, "Item", Global.QliteDefaults.XmlDefaults.NamespaceURI);
            newMenuNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Title"));
            newMenuNode.Attributes[0].Value = menu.Name;
            newMenuNode.Attributes.Append(Global.QliteDefaults.XmlDefaults.CreateAttribute("Type"));
            newMenuNode.Attributes[1].Value = menu.Index.ToString();
            return newMenuNode;
        }

        public static void AddMenu(QueryMenuItem inItem)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "QUERIES")
                { node.AppendChild(CreateMenuNode(inItem)); break; }

            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        public static void UpdateMenu(QueryMenuItem newItem, QueryMenuItem oldItem)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "MENUS")
                    foreach (XmlNode node2 in node.ChildNodes)
                        if (node2.Name.ToUpper() == "QUERYMENU")
                            foreach (XmlNode menuNode in node2.ChildNodes)
                                if (oldItem.Name.ToUpper() == menuNode.Attributes[0].Value.ToUpper())
                                { node2.ReplaceChild(CreateMenuNode(newItem), menuNode); goto Finish; }
        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        public static void DeleteMenu(QueryMenuItem inItem)
        {
            foreach (XmlNode node in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node.Name.ToUpper() == "MENUS")
                    foreach (XmlNode node2 in node.ChildNodes)
                        if (node2.Name.ToUpper() == "QUERYMENU")
                            foreach (XmlNode menuNode in node2.ChildNodes)
                                foreach (XmlAttribute att in menuNode.Attributes)
                                    if (att.Name.ToUpper() == "TITLE")
                                        if (att.Value.ToUpper() == inItem.Name.ToUpper())
                                        { node2.RemoveChild(menuNode); goto Finish; }
        Finish:
            using (FileStream WRITER = new FileStream(Global.QliteDefaults.XmlFileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
            { Global.QliteDefaults.XmlDefaults.Save(WRITER); WRITER.Close(); }
        }

        #endregion

        #region Help
        public string[,] PullHelpTopics()
        {
            string[,] holder = new string[CountTwoDeep("Help", "Topic"), 2];
            Global.QliteDefaults.XmlDefaults.Load("Qlite2.xml");

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node1.Name.ToUpper() == "HELP")
                {
                    int x = 0;
                    foreach (XmlNode node3 in node1.ChildNodes)
                        if (node3.Name.ToUpper() == "TOPIC")
                        {
                            XmlAttributeCollection nodeAttributes = node3.Attributes;
                            holder[x, 0] = nodeAttributes[0].Value.ToString();
                            holder[x, 1] = node3.InnerText;
                            x++;
                        }
                }

            return holder;

        }

        public string[,] PullHelpContent()
        {
            string[,] holder = new string[CountTwoDeep("Help", "Content"), 4];

            foreach (XmlNode node1 in Global.QliteDefaults.XmlDefaults.ChildNodes[0])
                if (node1.Name.ToUpper() == "HELP")
                {
                    int x = 0;
                    foreach (XmlNode node3 in node1.ChildNodes)
                        if (node3.Name.ToUpper() == "CONTENT")
                        {
                            XmlAttributeCollection nodeAttributes = node3.Attributes;
                            holder[x, 0] = nodeAttributes[0].Value.ToString();
                            holder[x, 1] = nodeAttributes[1].Value.ToString();
                            holder[x, 2] = node3.InnerText.ToString();
                            x++;
                        }
                }

            return holder;

        }

        #endregion

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

        ~QEngine_XML()
        {
            Dispose(false);
        }

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
