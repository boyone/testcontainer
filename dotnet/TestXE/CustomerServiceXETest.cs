using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using Testcontainers.Oracle;
using Xunit;

namespace Repository.Provider.Tests;

public sealed class CustomerServiceXETest : IAsyncLifetime
{
    private readonly OracleContainer _oracleContainer = new OracleBuilder()
        .WithImage("gvenzl/oracle-xe:18.4.0-slim-faststart")
        .WithUsername("APPUSER")
        .WithPassword("apppassword")
        .WithResourceMapping(new DirectoryInfo("../../../initialDb"), "/docker-entrypoint-initdb.d")
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
    public void ShouldReturnThreeCustomers()
    {
        // Arrange
        var customerService = new CustomerService(new XEConnectionProvider(_oracleContainer.GetConnectionString()));

        // Act
        var customers = customerService.GetCustomers();

        // Assert
        Assert.Equal(3, customers.Count());
    }

    [Fact]
    // [Trait(nameof(DockerCli.DockerPlatform), nameof(DockerCli.DockerPlatform.Linux))]
    public async Task ExecScriptReturnsSuccessful()
    {
        // Arrange
        const string scriptContent = "SELECT 1 FROM DUAL;";

        // Act
        var execResult = await _oracleContainer.ExecScriptAsync(scriptContent)
            .ConfigureAwait(false);

        // Assert
        Assert.True(0L.Equals(execResult.ExitCode), execResult.Stderr);
    }
}