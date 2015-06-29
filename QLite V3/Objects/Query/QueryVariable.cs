using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Enumerations;

namespace QLite.Objects.Query
{
    public class QueryVariable : IDisposable
    {
        public string Question { get; set; }
        public string Default { get; set; }
        public QAction Action { get; set; }

        public QueryVariable() { Action = QAction.None; }

        public bool CompareVariable(QueryVariable var)
        {
            if (var.Question != Question) return false;
            if (var.Default != Default) return false;
            return true;
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

        ~QueryVariable()
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
