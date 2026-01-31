using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;
using Supplus.Infrastructure.Data.Context;

namespace Supplus.Infrastructure.Data.Repositories;

public class BaseRepository<TEntidade> : IRepository<TEntidade> where TEntidade : EntidadeBase
{
    private readonly SupplusDbContext _dbContext;

    public BaseRepository(SupplusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AdicionarAsync(TEntidade entidade)
    {
        await _dbContext.Set<TEntidade>().AddAsync(entidade);
    }

    public async Task<TEntidade?> ObterPorIdAsync(long id)
    {
        return await _dbContext
            .Set<TEntidade>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntidade?> ObterPorIdExternoAsync(Guid idExterno)
    {
        return await _dbContext
            .Set<TEntidade>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdExterno == idExterno);
    }
}
