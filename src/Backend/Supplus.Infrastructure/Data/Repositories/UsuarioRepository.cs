using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;
using Supplus.Infrastructure.Data.Context;

namespace Supplus.Infrastructure.Data.Repositories;

public sealed class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
{
    private readonly SupplusDbContext _dbContext;

    public UsuarioRepository(SupplusDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteUsuarioComIdentificadorExternoAsync(Guid identificador)
    {
        return await _dbContext
            .Usuarios
            .AsNoTracking()
            .AnyAsync(u => u.IdExterno == identificador);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _dbContext
            .Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
