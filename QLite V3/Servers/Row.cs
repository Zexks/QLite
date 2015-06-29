using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QLite.Servers
{
    public class Row
    {
        public List<Column> Columns { get; set; }
        public int ColCount { get; set; }
        public string[] DataTypes { get; set; }
        public string[] Values { get; set; }

        public Row() { }

        public Row(DataRow row)
        {
            ColCount = row.ItemArray.Length;
            LoadDataTypes(row);
            LoadDataValues(row);
        }

        private void LoadDataTypes(DataRow row)
        {
            int count = 0;
            DataTable tmpTable = row.Table;
            foreach (DataColumn col in tmpTable.Columns)
                Columns.Add(new Column(col));
            DataTypes = new string[Columns.Count];

            foreach (Column col in Columns)
                DataTypes[count] = col.DataType;

        }

        private void LoadDataValues(DataRow row)
        {
            DataTable tmpTable = row.Table;
            Values = new string[Columns.Count];

            for (int x = 0; x < row.ItemArray.Length; x++)
                Values[x] = row.ItemArray[x].ToString();

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

        ~Row()
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
