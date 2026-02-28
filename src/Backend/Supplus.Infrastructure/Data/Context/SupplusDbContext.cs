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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Chamado> Chamados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasQueryFilter(u => u.Status == Status.Ativo);

            e.Property(u => u.Id)
                .HasColumnName("IdUsuario");

            e.Property<string>("_senha")
                .HasColumnName("Senha");

            e.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.Usuario)
                .HasForeignKey(rt => rt.IdUsuario);
        });

        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.Property(u => u.Id)
                .HasColumnName("IdRefreshToken");
        });

        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasQueryFilter(c => c.Status == Status.Ativo);
            
            e.Property(c => c.Id)
                .HasColumnName("IdCategoria");

            e.HasMany(c => c.Chamados)
                .WithOne(ch => ch.Categoria)
                .HasForeignKey(ch => ch.IdCategoria);

            // Seed Data
            e.HasData(
                new Categoria(nome: "Infraestrutura", descricao: "Problemas relacionados a rede, hardware e equipamentos."),
                new Categoria(nome: "Software", descricao: "Erros, bugs ou solicitações em sistemas e aplicativos."),
                new Categoria(nome: "Acesso", descricao: "Solicitações de criação de contas, permissões e recuperação de senha."),
                new Categoria(nome: "Suporte ao Usuário", descricao: "Dúvidas gerais, treinamento e auxílio no uso de ferramentas."),
                new Categoria(nome: "Segurança", descricao: "Incidentes de segurança, acessos indevidos e proteção de dados."),
                new Categoria(nome: "Serviços Administrativos", descricao: "Solicitações de materiais, crachá, reservas de sala e apoio administrativo."),
                new Categoria(nome: "Melhoria", descricao: "Sugestões de melhorias, novas funcionalidades e projetos internos."),
                new Categoria(nome: "Outros", descricao: "Chamados que não se enquadram nas categorias existentes."));

        });

        modelBuilder.Entity<Chamado>(e =>
        {
            e.HasQueryFilter(c => c.Status == Status.Ativo);

            e.Property(c => c.Id)
                .HasColumnName("IdChamado");

            e.HasOne(c => c.Agente)
                .WithMany(a => a.Chamados)
                .HasForeignKey(c => c.IdAgente);
        });
            
        base.OnModelCreating(modelBuilder);
    }
}