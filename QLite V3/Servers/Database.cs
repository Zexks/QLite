using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Engines;

namespace QLite.Servers
{
    public class Database
    {
        public string Name { get; set; }
        public string[] Parent { get; set; }
        public bool Mapped { get; set; }
        public List<Table> Tables { get; set; }
        public TableMap TBLMap { get; set; }

        public Database() { }

        public Database(string dbName)
        {
            Name = dbName;
            Parent = new string[2];
        }

        public void LoadTableSet(Server svr)
        {
            Tables = QInfo.GetTableList(Name, svr);
            TBLMap = new TableMap(Tables.Count);
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

        ~Database()
        { Dispose(false); }

        protected virtual void Dispose(bool disposedStatus)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposedStatus)
                { }
            }
        }

        #endregion
    }
}
