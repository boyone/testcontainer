using System.Data.Common;
using DotNet.Testcontainers.Containers;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Test;

public class InitialDataTest : IAsyncLifetime
{
    private PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:13.11-alpine3.17")
        .WithDatabase("WMSDEV")
        .WithUsername("wmscfg")
        .WithPassword("password")
        // .WithVolumeMount("./initialDb", "/docker-entrypoint-initdb.d")
        .WithResourceMapping(new DirectoryInfo("../../../initialDb"), "/docker-entrypoint-initdb.d")
        .Build();

    // private PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
    //     .WithImage("postgres:13.11-alpine3.17")
    //     .Build();

    public Task InitializeAsync()
    {
        return postgreSqlContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return postgreSqlContainer.DisposeAsync().AsTask();
    }

    [Fact]
    public void Test1()
    {
        using (DbConnection connection = new NpgsqlConnection(postgreSqlContainer.GetConnectionString()))
        {
            // Assert.Equal(TestcontainersHealthStatus.Healthy, postgreSqlContainer.Health);
            using (DbCommand command = new NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * from product";
                var list = command.ExecuteReader();
                var counter = 0;
                foreach (var row in list)
                {
                    counter++;
                }
                Assert.Equal(261, counter);
            }
        }
    }
}