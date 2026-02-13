using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Application.UseCases.Auth.Login;
using Supplus.Application.UseCases.Auth.RereshTokens;
using Supplus.Application.UseCases.Usuarios.Registrar;

namespace Supplus.Application;

public static class DIExtensions
{
    public static void AdicionaApplication(this IServiceCollection services)
    {
        ConfiguraUseCases(services);
        ConfiguraServices(services);
    }

    private static void ConfiguraUseCases(IServiceCollection services)
    {
        services
            .AddScoped<ILoginUseCase, LoginUseCase>()
            .AddScoped<IGerarRefreshTokenUseCase, GerarRefreshTokenUseCase>()
            .AddScoped<IRegistrarUsuarioUseCase, RegistrarUsuarioUseCase>();
    }

    private static void ConfiguraServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
    }
}
