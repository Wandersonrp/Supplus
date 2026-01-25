using Microsoft.IdentityModel.Tokens;
using Supplus.Domain.Enums;
using Supplus.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Supplus.Infrastructure.Services.Seguranca.Tokens.Jwt;

public class GeradorTokenJwt : JwtTokenHandler, IGeradorAccessToken
{
    private readonly string _chaveAssinatura;
    private readonly uint _expiracaoEmMinutos;

    public GeradorTokenJwt(string chaveAssinatura, uint expiracaoEmMinutos)
    {
        _chaveAssinatura = chaveAssinatura;
        _expiracaoEmMinutos = expiracaoEmMinutos;
    }

    public string Gerar(Guid identificador, Role role)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Sid, identificador.ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {            
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expiracaoEmMinutos),
            SigningCredentials = new SigningCredentials(ChaveSeguranca(_chaveAssinatura), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
