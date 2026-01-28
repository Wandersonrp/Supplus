using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Supplus.Infrastructure.Services.Seguranca.Tokens.Jwt;

public abstract class JwtTokenHandler
{
    protected SymmetricSecurityKey ChaveSeguranca(string chaveAssinatura) => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveAssinatura));
}
