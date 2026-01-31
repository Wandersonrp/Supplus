using Testcontainers.PostgreSql;

namespace Supplus.Api.Tests;

public class ContainersFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; set; }

    public ContainersFixture()
    {
        Container = new PostgreSqlBuilder(image: "postgres:latest")
            .Build();
    }

    public async Task InitializeAsync() => await Container.StartAsync();

    public async Task DisposeAsync() => await Container.StopAsync();
}
