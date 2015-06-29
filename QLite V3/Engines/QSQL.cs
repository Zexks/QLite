using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using QLite.Engines;

namespace QLite.Engines
{
    class QSQL
    {
        public static string HookUp(string machine, string instance, string user, string pass, string db)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            try
            {
                build.DataSource = machine + "\\" + instance;
                build.ConnectTimeout = 10;
                build.UserID = user;
                build.Password = pass;
                build.InitialCatalog = db;
            }
            catch (Exception ex) { QMain.ReportEx(ex, "QSQL", "HookUp"); }
            return build.ConnectionString;
        }

        public static string HookUp(string machine, string instance, string user, string pass, string db, int to)
        {
            SqlConnectionStringBuilder build = new SqlConnectionStringBuilder();
            try
            {
                build.DataSource = machine + "\\" + instance;
                build.ConnectTimeout = to;
                build.UserID = user;
                build.Password = pass;
                build.InitialCatalog = db;
            }
            catch (Exception ex) { QMain.ReportEx(ex, "QSQL", "HookUp"); }
            return build.ConnectionString;
        }

        public static bool CheckConn(string conn)
        {
            string[] split = conn.Split(';');
            SqlConnection tmpConn = new SqlConnection(HookUp(split[0], split[1], split[2], split[3], "master"));
            try
            {
                tmpConn.Open();
                tmpConn.Close();
            }
            catch (SqlException ex)
            {
                foreach (SqlError er in ex.Errors)
                    switch (er.Class)
                    {
                        case 20:
                            MessageBox.Show("Connection Failure", "Unable to connect to Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            QMain.ReportEx(ex, "QSQL", "CheckConn");
                            break;
                    }
                return false;
            }
            return true;
        }

        public static bool CheckConn(SqlConnection tmpConn)
        {
            bool value = true;
            try
            {
                tmpConn.Open();
                tmpConn.Close();
            }
            catch (SqlException ex)
            {
                QMain.ReportEx(ex, "QSQL", "CheckConn");
                value = false;
            }
            return value;
        }

    }
}
