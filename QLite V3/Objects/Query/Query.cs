using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Objects.Query;
using QLite.Enumerations;

namespace QLite.Objects.Query
{
    public class Query : IDisposable
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Parent { get; set; }
        public int Index { get; set; }
        public List<QueryVariable> Variables { get; set; }
        public QAction Action { get; set; }


        public Query()
        {
            Variables = new List<QueryVariable>();
            Action = new QAction();
        }

        public Query(string title)
        {
            Variables = new List<QueryVariable>();
            Title = title;
        }

        public bool CompareQuery(Query query)
        {
            if (Title != query.Title) return false;
            if (Text != query.Text) return false;
            if (Parent != query.Parent) return false;
            if (Index != query.Index) return false;
            if (query.Variables.Count != Variables.Count) return false;
            else for (int i = 0; i < Variables.Count; i++)
                    if (!Variables[i].CompareVariable(query.Variables[i])) return false;
            return true;
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

        ~Query()
        { Dispose(false); }

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
