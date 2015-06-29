using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLite.Servers;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLite.Engines
{
    public class QEngine_Info : IDisposable
    {
        public static List<Column> GetColumnList(SqlConnection conn, string table)
        {
            List<Column> clmList = new List<Column>();
            try
            {
                string dbQuery = "select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + table + "'";

                conn.Open();

                SqlCommand tmp1 = new SqlCommand(dbQuery, conn);
                SqlDataReader rdFirstSet = tmp1.ExecuteReader();

                while (rdFirstSet.Read())
                {
                    Column tmpTable = new Column();
                    tmpTable.Name = rdFirstSet[0].ToString();
                    clmList.Add(tmpTable);
                }

                conn.Close();
            }
            catch (Exception ex)
            { QEngine_Main.ReportEx(ex, "QEngine_Info", "GetColumnList"); }

            return clmList;
        }

        public static List<Table> GetTableList(string db, Server svr)
        {
            svr.ConnectorDB = db;
            SqlConnection conn = svr.Connector;
            List<Table> tblList = new List<Table>();
            try
            {
                string dbQuery = "SELECT table_name, table_schema FROM Information_Schema.Tables where Table_Type = 'BASE TABLE' ORDER BY table_name";

                conn.Open();

                SqlCommand tmp1 = new SqlCommand(dbQuery, conn);
                SqlDataReader rdFirstSet = tmp1.ExecuteReader();

                while (rdFirstSet.Read())
                {
                    Table tmpTable = new Table();
                    tmpTable.TableName = rdFirstSet[0].ToString();
                    tmpTable.TableSchema = rdFirstSet[1].ToString();
                    tblList.Add(tmpTable);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                QEngine_Main.ReportEx(ex, "QEngine_Info", "GetTableList");
            }
            return tblList;
        }

        public static List<Database> GetDBList(SqlConnection conn)
        {
            List<Database> dbList = new List<Database>();
            try
            {
                string dbQuery = "use master exec sp_databases";
                conn.Open();

                SqlCommand tmp1 = new SqlCommand(dbQuery, conn);
                SqlDataReader rdFirstSet = tmp1.ExecuteReader();

                while (rdFirstSet.Read())
                    if (rdFirstSet[0].ToString() != null)
                    { Database newDB = new Database(rdFirstSet[0].ToString()); dbList.Add(newDB); }

                conn.Close();
            }
            catch (Exception ex)
            { QEngine_Main.ReportEx(ex, "QEngine_Info", "GetDBList"); }
            return dbList;
        }

        public static string[] GetTreeNodeDepth(ref TreeView tmpView)
        {
            string[] parents = new string[0];
            TreeNode[] parentNodes = new TreeNode[5];
            if (tmpView.Nodes.Count > 0)
            {
                if (tmpView.SelectedNode.Parent != null)
                {
                    TreeNode firstParent = tmpView.SelectedNode.Parent;
                    parentNodes[0] = new TreeNode(firstParent.Text);
                    if (firstParent.Parent != null)
                    {
                        TreeNode secondParent = firstParent.Parent;
                        parentNodes[1] = new TreeNode(secondParent.Text);
                        if (secondParent.Parent != null)
                        {
                            TreeNode thirdParent = secondParent.Parent;
                            parentNodes[2] = new TreeNode(thirdParent.Text);
                            if (thirdParent.Parent != null)
                            {
                                TreeNode fourthParent = thirdParent.Parent;
                                parentNodes[3] = new TreeNode(fourthParent.Text);
                                if (fourthParent.Parent != null)
                                {
                                    TreeNode fifthParent = fourthParent.Parent;
                                    parentNodes[4] = new TreeNode(fifthParent.Text);
                                    parents = new string[5];
                                    for (int i = 0; i < parents.Length; i++)
                                    {
                                        parents[i] = parentNodes[i].Text;
                                    }
                                    return parents;
                                }
                                parents = new string[4];
                                for (int i = 0; i < parents.Length; i++)
                                {
                                    parents[i] = parentNodes[i].Text;
                                }
                                return parents;
                            }
                            parents = new string[3];
                            for (int i = 0; i < parents.Length; i++)
                            {
                                parents[i] = parentNodes[i].Text;
                            }
                            return parents;
                        }
                        parents = new string[2];
                        for (int i = 0; i < parents.Length; i++)
                        {
                            parents[i] = (string)parentNodes[i].Text;
                        }
                        return parents;
                    }
                    parents = new string[1];
                    for (int i = 0; i < parents.Length; i++)
                    {
                        parents[i] = parentNodes[i].Text;
                    }
                    return parents;
                }
            }
            return parents;
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

        ~QEngine_Info()
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
