using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;
using Xunit;

namespace Repository.Provider.Tests;

public sealed class CustomerServiceNPGTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:13.11-alpine3.17")
        .WithDatabase("store")
        .WithUsername("myadmin")
        .WithPassword("password")
        .WithResourceMapping(new DirectoryInfo("../../../initialDb"), "/docker-entrypoint-initdb.d")
        .Build();

    public Task InitializeAsync()
    {
        return _postgres.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }

    [Fact]
    public void ShouldReturnTwoCustomers()
    {
        // Arrange
        var customerService = new CustomerService(new PostgresqlConnectionProvider(_postgres.GetConnectionString()));

        // Act
        customerService.Create(new Customer("George"));
        var customers = customerService.GetCustomers();

        // Assert
        Assert.Equal(4, customers.Count());
    }
}