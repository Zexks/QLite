using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using QLite.Engines;

namespace QLite.Controls
{

    public class QGrid : DataGridView
    {
        public QGrid()
        {
            this.DataError += new DataGridViewDataErrorEventHandler(QliteGrid_DataError);
        }

        public void SetDataTable(DataTable table)
        {
            this.DataSource = table;
            try
            {
                for (int i = 0; i < table.Columns.Count; i++)
                    if (i < this.Columns.Count)
                        if (this.Columns[i].ValueType == typeof(DateTime))  //Format Date and Time Columns
                        {
                            DataGridViewCellStyle dgvCellStyle = new DataGridViewCellStyle();
                            dgvCellStyle.Format = "M/d/yyyy  hh:mm:ss tt";
                            this.Columns[i].DefaultCellStyle = dgvCellStyle;
                        }

                this.DataSource = table;
            }
            catch (Exception ex)
            { QMain.ReportEx(ex, "Engine", "SetDataTable"); }
        }

        private void QliteGrid_DataError(object sender, EventArgs e)
        {

        }
    }

}
