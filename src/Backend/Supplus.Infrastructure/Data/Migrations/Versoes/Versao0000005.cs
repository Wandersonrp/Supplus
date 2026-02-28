using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.CRIAR_SEED_CATEGORIA, "Adiciona Seed de Categoria")]
public class Versao0000005 : ForwardOnlyMigration
{    
    public override void Up()
    {
        var criadoEm = DateTime.UtcNow;

        Insert.IntoTable("Categorias")
            .Rows(
                new 
                { 
                    Nome = "Infraestrutura", 
                    Descricao = "Problemas relacionados a rede, hardware e equipamentos.", 
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {
                    Nome = "Software",
                    Descricao = "Erros, bugs ou solicitações em sistemas e aplicativos.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {
                    Nome = "Suporte ao Usuário",
                    Descricao = "Dúvidas gerais, treinamento e auxílio no uso de ferramentas.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {                    
                    Nome = "Segurança",
                    Descricao = "Incidentes de segurança, acessos indevidos e proteção de dados.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {                    
                    Nome = "Serviços Administrativos",
                    Descricao = "Solicitações de materiais, crachá, reservas de sala e apoio administrativo.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {                    
                    Nome = "Melhoria",
                    Descricao = "Sugestões de melhorias, novas funcionalidades e projetos internos.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                },
                new
                {                    
                    Nome = "Outros",
                    Descricao = "Chamados que não se enquadram nas categorias existentes.",
                    IdExterno = Guid.NewGuid(),
                    CriadoEm = criadoEm,
                    Status = 1
                }
            );                                
    }
}
