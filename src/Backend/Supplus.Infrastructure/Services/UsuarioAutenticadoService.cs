using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Models;
using Supplus.Domain.Services;
using Supplus.Domain.Services.Tokens;
using Supplus.Infrastructure.Data.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Supplus.Infrastructure.Services;

public class UsuarioAutenticadoService : IUsuarioAutenticado
{
    private readonly SupplusDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public UsuarioAutenticadoService(SupplusDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<UsuarioAutenticado> ObterUsuarioAutenticadoAsync()
    {
        var token  = _tokenProvider.ObterToken();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var identificador = jwtToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        var identificadorUsuario = Guid.Parse(identificador);

        return await _dbContext.Usuarios
            .AsNoTracking()
            .Where(u => u.IdExterno == identificadorUsuario)
            .Select(u => new UsuarioAutenticado(u.Id, u.Email, u.Role))
            .FirstAsync();
    }
}
