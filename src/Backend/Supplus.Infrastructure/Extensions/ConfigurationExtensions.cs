using Microsoft.Extensions.Configuration;

namespace Supplus.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("PostgreSql") ??
            throw new Exception("Adicione a string de conexão 'PostgreSql'");
    }

    public static bool EAmbienteTeste(this IConfiguration configuration) => configuration.GetValue<bool>("Teste");
}
