using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Engines;

namespace QLite.Servers
{
    public class Table
    {
        public string TableName { get; set; }
        public string TableSchema { get; set; }
        public string HomeDatabase { get; set; }
        public bool Mapped { get; set; }
        public List<Row> Rows { get; set; }
        public List<Column> Columns { get; set; }
        public ColumnMap ColMap { get; set; }

        public Table() { }

        public Table(string tblName)
        {
            TableName = tblName;
        }

        public void LoadColumnSet(Server svr)
        {
            Columns = QInfo.GetColumnList(svr.Connector, TableName);
            ColMap = new ColumnMap(Columns.Count);
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

        ~Table()
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
