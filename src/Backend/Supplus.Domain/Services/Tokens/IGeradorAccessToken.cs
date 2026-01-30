using Supplus.Domain.Enums;

namespace Supplus.Domain.Services.Tokens;

public interface IGeradorAccessToken
{
    string Gerar(Guid identificador, Role role);
}
