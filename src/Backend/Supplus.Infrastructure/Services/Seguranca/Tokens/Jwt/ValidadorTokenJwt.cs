using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Supplus.Domain.Services.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Supplus.Infrastructure.Services.Seguranca.Tokens.Jwt;

public class ValidadorTokenJwt : JwtTokenHandler, IValidadorAccessToken
{
    private readonly string _chaveAssinatura;

    public ValidadorTokenJwt(string chaveAssinatura)
    {
        _chaveAssinatura = chaveAssinatura;
    }

    public Guid ValidarEObterIdUsuario(string token)
    {
        var parametros = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = ChaveSeguranca(_chaveAssinatura),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, parametros, out _);

        var identificadorUsuario = principal
            .Claims
            .First(c => c.Type == ClaimTypes.Sid)
            .Value;

        return Guid.Parse(identificadorUsuario);
    }
}
