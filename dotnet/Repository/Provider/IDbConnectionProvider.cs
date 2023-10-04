using System.Data.Common;

namespace Repository.Provider;

public interface IDbConnectionProvider
{
    DbConnection GetConnection();
}