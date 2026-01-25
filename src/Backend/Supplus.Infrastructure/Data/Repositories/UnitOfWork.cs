using Supplus.Domain.Repositories;
using Supplus.Infrastructure.Data.Context;

namespace Supplus.Infrastructure.Data.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SupplusDbContext _dbContext;

    public UnitOfWork(SupplusDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitAsync() => await _dbContext.SaveChangesAsync();    
}
