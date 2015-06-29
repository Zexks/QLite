using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using QLite.Engines;

namespace QLite.Servers
{
    public class Server : IDisposable
    {
        public List<Database> Databases { get; set; }
        public DatabaseMap DBMap { get; set; }
        public bool Mapped { get; set; }
        public string InfoMessages { get; set; }
        public string ErrorMessages { get; set; }
        public string InstanceID { get; set; }
        public string Name { get; set; }
        public string Machine { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public int TimeOut { get; set; }
        public SqlConnection Connector { get; set; }

        public Server() { }

        public Server(Server svr)
        {
            Databases = svr.Databases;
            DBMap = svr.DBMap;
            Mapped = svr.Mapped;
            InfoMessages = svr.InfoMessages;
            ErrorMessages = svr.ErrorMessages;
            InstanceID = svr.InstanceID;
            Name = svr.Name;
            Machine = svr.Machine;
            UserID = svr.UserID;
            Password = svr.Password;
            ConnectionString = svr.ConnectionString;
            TimeOut = svr.TimeOut;
            Connector = svr.Connector;
        }

        public Server(string connectionString)
        {
            ConnectionString = connectionString;
            ParseConnectionString(connectionString);
            Connector = new SqlConnection(QSQL.HookUp(Machine, Name, UserID, Password, "master", TimeOut));
        }

        public Server(string mach, string inst, string user, string pass, int to)
        {
            Machine = mach;
            Name = inst;
            UserID = user;
            Password = pass;
            TimeOut = to;
            ConnectionString = inst + ";" + mach + ";" + user + ";" + pass + ";" + to;
            Connector.ConnectionString = QSQL.HookUp(Machine, Name, UserID, Password, "master", TimeOut);
        }

        public string ConnectorDB
        {
            get { return Connector.Database; }
            set { Connector.ConnectionString = QSQL.HookUp(Machine, Name, UserID, Password, value, TimeOut); }
        }

        public string ServerString
        {
            get { return Connector.ConnectionString; }
            set { ParseConnectionString(value); }
        }

        public void initConnector()
        {
            Connector.InfoMessage += new SqlInfoMessageEventHandler(Connector_InfoMessageHandler);
        }

        public void UpdateConnection(string connectionString)
        {
            ParseConnectionString(connectionString + ", 10");
        }

        public void LoadDatabaseSet()
        {
            Databases = QInfo.GetDBList(Connector);
            DBMap = new DatabaseMap(Databases.Count);
        }

        private void ParseConnectionString(string connectionString)
        {
            string[] parts = connectionString.Split(';');
            InstanceID = parts[0] + "\\" + parts[1];
            Machine = parts[0];
            Name = parts[1];
            UserID = parts[2];
            Password = parts[3];
            TimeOut = (parts.Length < 4) ? Convert.ToInt32(parts[4]) : 10;
        }

        private void Connector_InfoMessageHandler(object sender, SqlInfoMessageEventArgs e)
        {
            InfoMessages = InfoMessages + e.Message + "\r\n";
            ErrorMessages = ErrorMessages + e.Errors + "\r\n";
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

        ~Server()
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
