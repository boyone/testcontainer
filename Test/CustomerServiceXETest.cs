using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using Testcontainers.Oracle;
using Testcontainers.PostgreSql;
using Xunit;

namespace Repository.Provider.Tests;

public sealed class CustomerServiceXETest : IAsyncLifetime
{
    private readonly OracleContainer _oracleContainer = new OracleBuilder()
        .WithImage("gvenzl/oracle-xe:18.4.0-slim-faststart")
        .Build();

    public Task InitializeAsync()
    {
        return _oracleContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _oracleContainer.DisposeAsync().AsTask();
    }

    [Fact]
    public void ShouldReturnTwoCustomers()
    {
        // Given
        var customerService = new CustomerService(new XEConnectionProvider(_oracleContainer.GetConnectionString()));

        // When
        customerService.Create(new Customer("George"));
        customerService.Create(new Customer("John"));
        var customers = customerService.GetCustomers();

        // Then
        Assert.Equal(2, customers.Count());
    }
}