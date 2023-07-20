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
        .WithEnvironment("ORACLE_PASSWORD", "syspassword")
        .WithEnvironment("APP_USER", "APPUSER")
        .WithEnvironment("APP_USER_PASSWORD", "apppassword")
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
    public void ShouldReturnTwoCustomers()
    {
        // Given
        var customerService = new CustomerService(new XEConnectionProvider(_oracleContainer.GetConnectionString()));

        // When
        customerService.Create(new Customer("George"));
        customerService.Create(new Customer("John"));
        var customers = customerService.GetCustomers();

        // Then
        Assert.Equal(5, customers.Count());
    }

    [Fact]
    // [Trait(nameof(DockerCli.DockerPlatform), nameof(DockerCli.DockerPlatform.Linux))]
    public async Task ExecScriptReturnsSuccessful()
    {
        // Given
        const string scriptContent = "SELECT 1 FROM DUAL;";

        // When
        var execResult = await _oracleContainer.ExecScriptAsync(scriptContent)
            .ConfigureAwait(false);

        // When
        Assert.True(0L.Equals(execResult.ExitCode), execResult.Stderr);
    }
}