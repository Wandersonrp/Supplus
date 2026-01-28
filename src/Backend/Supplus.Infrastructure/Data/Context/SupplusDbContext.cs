using Microsoft.EntityFrameworkCore;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;

namespace Supplus.Infrastructure.Data.Context;

public class SupplusDbContext : DbContext
{
    public SupplusDbContext(DbContextOptions<SupplusDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasQueryFilter(u => u.Status == Status.Ativo);

            e.Property(u => u.Id)
                .HasColumnName("IdUsuario");

            e.Property<string>("_senha")
                .HasColumnName("Senha");
        });
            
        base.OnModelCreating(modelBuilder);
    }
}
