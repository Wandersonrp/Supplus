using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Infrastructure.Data.Context;
using System.Reflection;

namespace Supplus.Infrastructure;

public static class DIExtensions
{
    public static void AdicionaInrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ConfiguraDbContext(services, configuration);
        ConfiguraFluentMigrator(services, configuration);
    }
    
    private static void ConfiguraDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql") ??
            throw new Exception("Adicione a string de conexão 'PostgreSql'");

        services.AddDbContext<SupplusDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
    }

    private static void ConfiguraFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql") ??
            throw new Exception("Adicione a string de conexão 'PostgreSql'");

        services.AddFluentMigratorCore().ConfigureRunner(opt =>
        {
            opt.AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("Supplus.Infrastructure"))
                .For
                .All();
        });
    }
}
