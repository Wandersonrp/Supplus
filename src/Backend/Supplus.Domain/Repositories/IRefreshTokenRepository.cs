using Supplus.Domain.Entities;

namespace Supplus.Domain.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> ObterRefreshTokenPorTokenAsync(string token, CancellationToken cancellationToken);
    Task RevogarTodosRefreshTokensDoUsuarioAsync(long idUsuario); 
}
