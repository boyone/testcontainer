using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace Repository.Provider;

public class XEConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public XEConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new OracleConnection(_connectionString);
    }
}