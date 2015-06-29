using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Engines;
using QLite.Enumerations;
using QLite.Objects.Query;
using System.Xml;

namespace QLite.Objects.Query
{
    public class QueryMenuItem : IDisposable
    {
        public int ID { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public List<Query> QuerySet { get; set; }
        public QAction Action { get; set; }

        public bool Expanded { get; set; }

        public QueryMenuItem()
        {
            QuerySet = new List<Query>();
            Action = QAction.None;
        }

        public void Load_QuerySet()
        {
            List<Query> tmpQuerySet = new List<Query>();
            XmlNode QueriesNode = QXML.PullQueries();
            for (int i = 0; i < QueriesNode.ChildNodes.Count; i++)
            {
                Query tmpQuery = CreateQuery(QueriesNode.ChildNodes[i]);
                tmpQuery.ID = i;
                if (tmpQuery.Parent == Index) QuerySet.Add(tmpQuery);
            }

            AlignQueries();
        }

        public void Update_QuerySet()
        {
            for (int i = 0; i < QuerySet.Count; i++)
            {
                if (QuerySet[i].Action != QAction.Delete)
                {
                    if (QuerySet[i].Index != i)
                    {
                        if (QuerySet[i].Action != QAction.Add) QuerySet[i].Action = QAction.Update;
                        QuerySet[i].Index = i;
                    }
                    if (QuerySet[i].Parent != Index)
                    {
                        if (QuerySet[i].Action != QAction.Add) QuerySet[i].Action = QAction.Update;
                        QuerySet[i].Parent = Index;
                    }
                }
                else i = QuerySet.Count;
            }
        }

        public void AlignQueries()
        {
            int x = 0;
            List<Query> tmpList = new List<Query>();
            for (int i = 0; i < QuerySet.Count; i++)
                if (QuerySet[i].Index == x)
                { tmpList.Add(QuerySet[i]); x++; i = -1; }
            QuerySet.Clear();
            QuerySet = tmpList;
        }

        public bool CompareItem(QueryMenuItem item)
        {
            if (Index != item.Index) return false;
            if (Name != item.Name) return false;
            return true;
        }

        public bool CompareQuerySet(QueryMenuItem item)
        {
            if (item.QuerySet.Count != QuerySet.Count) return false;
            else for (int i = 0; i < QuerySet.Count; i++)
                    if (!QuerySet[i].CompareQuery(item.QuerySet[i])) return false;
            return true;
        }

        public Query CreateQuery(XmlNode QueryNode)
        {
            Query tmpQuery = new Query();
            foreach (XmlAttribute att in QueryNode.Attributes)
                switch (att.Name.ToUpper())
                {
                    case "TITLE":
                        tmpQuery.Title = att.Value;
                        break;
                    case "TYPE":
                        tmpQuery.Parent = Convert.ToInt32(att.Value);
                        break;
                    case "INDEX":
                        tmpQuery.Index = Convert.ToInt32(att.Value);
                        break;
                }

            if (QueryNode.ChildNodes.Count > 1)
                for (int i = 1; i < QueryNode.ChildNodes.Count; i++)
                    tmpQuery.Variables.Add(CreateVariable(QueryNode.ChildNodes[i]));

            tmpQuery.Text = QueryNode.FirstChild.InnerText;
            return tmpQuery;
        }

        private QueryVariable CreateVariable(XmlNode VarNode)
        {
            QueryVariable tmpVar = new QueryVariable();
            tmpVar.Default = VarNode.Attributes[0].Value;
            tmpVar.Question = VarNode.InnerText;
            return tmpVar;
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

        ~QueryMenuItem()
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
