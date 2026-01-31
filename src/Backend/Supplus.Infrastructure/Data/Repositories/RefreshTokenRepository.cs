using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;
using Supplus.Infrastructure.Data.Context;

namespace Supplus.Infrastructure.Data.Repositories;

public sealed class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
{
    private readonly SupplusDbContext _dbContext;

    public RefreshTokenRepository(SupplusDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Recupera do banco de dados o RefreshToken correspondente ao valor informado.    
    /// </summary>
    /// <param name="token">Valor do refresh token a ser buscado.</param>
    /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
    /// <returns>
    /// Uma instância de <see cref="RefreshToken"/> quando encontrada; caso contrário, null.
    /// </returns>
    public async Task<RefreshToken?> ObterRefreshTokenPorTokenAsync(string token, CancellationToken cancellationToken)
    {
        return await _dbContext
            .RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    /// <summary>
    /// Revoga todos os refresh tokens ativos do usuário especificado.
    /// </summary>
    /// <param name="idUsuario">Identificador do usuário cujos refresh tokens serão revogados.</param>
    /// <remarks>
    /// Busca no banco todos os refresh tokens do usuário que ainda não foram usados nem revogados
    /// e marca cada um como revogado. Esta operação modifica as entidades em memória; é necessário
    /// chamar <c>SaveChangesAsync</c> no contexto para persistir as alterações.
    /// </remarks>
    public async Task RevogarTodosRefreshTokensDoUsuarioAsync(long idUsuario)
    {
        // Busca todos os refresh tokens não usados e não revogados do usuário 
        var tokens = await _dbContext.RefreshTokens
            .Where(rt => rt.IdUsuario == idUsuario && !rt.FoiRevogado && !rt.FoiUsado)
            .ToListAsync();

        foreach (var token in tokens)
            token.Revogar();
    }

}
