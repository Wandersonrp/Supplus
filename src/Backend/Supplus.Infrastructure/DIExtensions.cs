using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Application.Services.Notificacoes;
using Supplus.Domain.Repositories;
using Supplus.Domain.Services;
using Supplus.Domain.Services.Tokens;
using Supplus.Infrastructure.Configurations;
using Supplus.Infrastructure.Data.Context;
using Supplus.Infrastructure.Data.Repositories;
using Supplus.Infrastructure.Extensions;
using Supplus.Infrastructure.Services;
using Supplus.Infrastructure.Services.Notificacoes.Email;
using Supplus.Infrastructure.Services.Seguranca.Tokens.Jwt;
using System.Reflection;

namespace Supplus.Infrastructure;

public static class DIExtensions
{
    public static void AdicionaInrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if(!configuration.EAmbienteTeste())
        {
            ConfiguraDbContext(services, configuration);
            ConfiguraFluentMigrator(services, configuration);
        }
        
        ConfiguraRepositorios(services);
        ConfiguraServicos(services, configuration);

        services.Configure<EmailConfig>(configuration.GetSection("Config:EmailConfig"));
    }
    
    private static void ConfiguraDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql") ??
            throw new Exception(configuration.ConnectionString());

        services.AddDbContext<SupplusDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
    }

    private static void ConfiguraFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {        
        services.AddFluentMigratorCore().ConfigureRunner(opt =>
        {
            opt.AddPostgres()
                .WithGlobalConnectionString(configuration.ConnectionString())
                .ScanIn(Assembly.Load("Supplus.Infrastructure"))
                .For
                .All();
        });
    }

    private static void ConfiguraRepositorios(IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
            .AddScoped<IUsuarioRepository, UsuarioRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    private static void ConfiguraServicos(IServiceCollection services, IConfiguration configuration)
    {      
        var chaveAssinatura = configuration["Config:Jwt:ChaveAssinatura"] ??
            throw new ArgumentException("Chave de assinatura JWT não encontrada na configuração.");

        uint expiracaoEmMinutos = uint.Parse(configuration["Config:Jwt:ExpiracaoEmMinutos"] ?? 
            throw new ArgumentException("Expiração em minutos não encontrada na configuração."));

        services
            .AddScoped<IGeradorAccessToken>(_ => new GeradorTokenJwt(chaveAssinatura, expiracaoEmMinutos))
            .AddScoped<IValidadorAccessToken>(_ => new ValidadorTokenJwt(chaveAssinatura))
            .AddScoped<IUsuarioAutenticado, UsuarioAutenticadoService>()
            .AddScoped<IEmailService, EmailService>();
    }    
}
