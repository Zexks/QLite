using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLite.Servers
{
    public class DatabaseMap
    {
        public string Parent { get; set; }
        public string[] Map { get; set; }

        public DatabaseMap() { }

        public DatabaseMap(int size)
        {
            Map = new string[size];
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

        ~DatabaseMap()
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
