using Supplus.Domain.Enums;

namespace Supplus.Domain.Services;

public interface IGeradorAccessToken
{
    string Gerar(Guid identificador, Role role);
}
