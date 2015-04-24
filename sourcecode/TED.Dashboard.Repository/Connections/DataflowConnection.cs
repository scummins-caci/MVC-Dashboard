using System;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;

namespace TED.Dashboard.Repository.Connections
{
    public class DataflowConnection : IConnection, IDisposable
    {
        private OracleConnection connection = null;
        private readonly string connectString;

        public DataflowConnection()
        {
            connectString = ConfigurationManager.AppSettings["DataflowConnectString"];
        }

        public OracleConnection OpenConnection()
        {
            connection = new OracleConnection(connectString);
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
