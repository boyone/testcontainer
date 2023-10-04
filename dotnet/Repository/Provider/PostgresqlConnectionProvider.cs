using System.Data;
using System.Data.Common;

using Npgsql;

namespace Repository.Provider;

public sealed class PostgresqlConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public PostgresqlConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
