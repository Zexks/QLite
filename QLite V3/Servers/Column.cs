using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QLite.Servers
{
    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool PrimaryKey { get; set; }

        public Column() { }

        public Column(string name, string dtype, bool key)
        {
            Name = name;
            DataType = dtype;
            PrimaryKey = key;
        }

        public Column(DataColumn col)
        {
            Name = col.ColumnName;
            DataType = col.DataType.ToString();

            foreach (DataColumn tmpCol in col.Table.PrimaryKey)
                if (tmpCol.ColumnName == Name)
                    PrimaryKey = true;
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

        ~Column()
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
