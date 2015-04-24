using System;
using System.Data;
using HighView.Framework;
using Oracle.DataAccess.Client;

namespace TED.Dashboard.Repository.Connections
{
    public class HV5Connection : IConnection, IDisposable
    {
        private OracleConnection connection;
        public OracleConnection OpenConnection()
        {
            connection = Session.GetSession().OpenConnection();
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
