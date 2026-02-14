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

    /// <summary>
    /// Obtém de forma assíncrona os dados do usuário autenticado a partir do token JWT presente na requisição.
    /// </summary>
    /// <remarks>
    /// O método extrai o token via <c>_tokenProvider</c>, interpreta-o com <c>JwtSecurityTokenHandler</c>,
    /// recupera o identificador único do usuário dos claims e consulta o banco de dados para retornar
    /// um objeto <see cref="UsuarioAutenticado"/> contendo Id, e-mail e perfil de acesso.
    /// </remarks>
    /// <returns>
    /// Uma tarefa que resulta em um <see cref="UsuarioAutenticado"/> representando o usuário autenticado.
    /// </returns>
    public async Task<UsuarioAutenticado> ObterUsuarioAutenticadoAsync(CancellationToken cancellationToken)
    {
        var token  = _tokenProvider.ObterToken();

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var identificador = jwtToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        var identificadorUsuario = Guid.Parse(identificador);

        return await _dbContext.Usuarios
            .AsNoTracking()
            .Where(u => u.IdExterno == identificadorUsuario)
            .Select(u => new UsuarioAutenticado(u.Id, u.Email, u.Role, u.Nome, u.Sobrenome, u.IdExterno))
            .FirstAsync(cancellationToken);
    }
}
