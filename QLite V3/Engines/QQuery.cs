using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using QLite.Servers;

namespace QLite.Engines
{
    class QQuery
    {
        public static BackgroundWorker bgwQueryRunner { get; set; }

        public static DataSet ExecuteQuery(Server svr, string query)
        {
            DataSet tmpSet = new DataSet();
            string[] pQuery = ParseQuery(query);
            svr.Connector.Open();

            for (int i = 0; i < pQuery.Length; i++)
                if (bgwQueryRunner.CancellationPending) i = pQuery.Length;
                else
                {
                    try
                    {
                        DataTable tmpTable = new DataTable();
                        SqlCommand sqlCmd = new SqlCommand(pQuery[i], svr.Connector);
                        sqlCmd.CommandTimeout = 0;
                        SqlDataAdapter tmpCom = new SqlDataAdapter(sqlCmd);
                        tmpCom.Fill(tmpTable);
                        if (tmpTable.Columns.Count > 0) tmpSet.Tables.Add(tmpTable);

                    }
                    catch (Exception ex)
                    { QMain.ReportEx(ex, "QQuery", "ExecuteQuery"); svr.Connector.Close(); }
                }

            svr.Connector.Close();
            return tmpSet;
        }
        
        public static string[] ParseQuery(string source)
        {
            string wrkSource = source.ToUpper();
            List<string> parts = new List<string>();
            int occurs = wrkSource.IndexOf("\nGO");

            if (occurs != -1)
                while (occurs != -1)
                {
                    string part = wrkSource.Substring(0, wrkSource.IndexOf("\nGO"));
                    if (part.Length + 4 < wrkSource.Length) wrkSource = wrkSource.Substring(part.Length + 4);
                    else wrkSource = wrkSource.Substring(part.Length + 3);
                    occurs = wrkSource.IndexOf("\nGO");
                    parts.Add(part);
                }
            else parts.Add(wrkSource);


            string[] final = new string[parts.Count];
            parts.CopyTo(final);
            return final;
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

        ~QQuery()
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
