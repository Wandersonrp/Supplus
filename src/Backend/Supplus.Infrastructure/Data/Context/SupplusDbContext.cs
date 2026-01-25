using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Entities;

namespace Supplus.Infrastructure.Data.Context;

public class SupplusDbContext : DbContext
{
    public SupplusDbContext(DbContextOptions<SupplusDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
}
