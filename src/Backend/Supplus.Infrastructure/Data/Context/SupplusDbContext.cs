using Microsoft.EntityFrameworkCore;

namespace Supplus.Infrastructure.Data.Context;

public class SupplusDbContext : DbContext
{
    public SupplusDbContext(DbContextOptions<SupplusDbContext> options) : base(options)
    {
    }
}
