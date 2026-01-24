using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Supplus.Infrastructure.Data.Migrations;

public static class ControleMigration
{
    /// <summary>
    /// Cria o banco de dados, caso não exista, e aplica as migrations pendentes utilizando o provedor de serviços.
    /// </summary>
    /// <param name="connectionString">
    /// String de conexão utilizada para verificar e criar o banco de dados.
    /// </param>
    /// <param name="serviceProvider">
    /// Provedor de serviços usado para executar as migrations configuradas.
    /// </param>
    public static void AplicarMigration(string connectionString, IServiceProvider serviceProvider)
    {
        CriarBanco(connectionString);
        // AplicarMigration(serviceProvider);
    }

    /// <summary>
    /// Aplica as migrations pendentes no banco de dados utilizando o serviço de migração configurado.
    /// </summary>
    /// <param name="serviceProvider">
    /// Provedor de serviços usado para obter a instância do <see cref="IMigrationRunner"/>.
    /// </param>
    private static void AplicarMigration(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        if (runner.HasMigrationsToApplyUp())
            runner.MigrateUp();
    }

    /// <summary>
    /// Cria um banco de dados PostgreSQL caso ele não exista, utilizando a string de conexão fornecida.
    /// </summary>
    /// <param name="connectionString">
    /// String de conexão para o servidor PostgreSQL.
    /// </param>
    private static void CriarBanco(string connectionString)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

        var nomeDatabase = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("Database");

        using var connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

        var parametros = new DynamicParameters();
        parametros.Add("name", nomeDatabase);

        var existe = connection.QueryFirstOrDefault<bool>("SELECT 1 FROM pg_database WHERE datname = @name;", parametros);

        if (!existe)
            connection.Execute($"CREATE DATABASE {nomeDatabase}");
    }
}
