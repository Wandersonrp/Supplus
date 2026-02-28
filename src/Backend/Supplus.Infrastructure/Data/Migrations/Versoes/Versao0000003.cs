using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.CRIAR_TABELA_CATEGORIA, "Cria tabela de Categoria")]
public class Versao0000003 : VersaoBase
{
    public override void Up()
    {
        CriarTabela(tabela: "Categorias", nomeId: "IdCategoria")
            .WithColumn("Nome").AsString(50).NotNullable()
            .WithColumn("Descricao").AsString(100).NotNullable();                                   
    }
}