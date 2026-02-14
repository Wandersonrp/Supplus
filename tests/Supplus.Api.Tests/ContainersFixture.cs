using Testcontainers.PostgreSql;

namespace Supplus.Api.Tests;

/// <summary>
/// Fixture responsável pelo gerenciamento do ciclo de vida do container Docker do PostgreSQL nos testes.
/// Implementa <see cref="IAsyncLifetime"/> para garantir que o container seja iniciado e finalizado de forma assíncrona.
/// </summary>
public class ContainersFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; set; }

    public ContainersFixture()
    {
        Container = new PostgreSqlBuilder(image: "postgres:latest")
            .Build();
    }

    /// <summary>
    /// Inicializa o container do PostgreSQL antes da execução dos testes.
    /// </summary>
    public async Task InitializeAsync() => await Container.StartAsync();

    /// <summary>
    /// Encerra e libera os recursos do container após a conclusão dos testes.
    /// </summary>
    public async Task DisposeAsync() => await Container.StopAsync();
}
