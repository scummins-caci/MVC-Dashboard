using Oracle.DataAccess.Client;

namespace TED.Dashboard.Repository.Connections
{
    public interface IConnection
    {
        OracleConnection OpenConnection();
        void Dispose();
    }
}
