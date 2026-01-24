using Microsoft.Extensions.Configuration;

namespace Supplus.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string ConnectioString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("PostgreSql") ??
            throw new Exception("Adicione a string de conexão 'PostgreSql'");
    }
}
