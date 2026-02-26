using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.CRIAR_TABELA_CHAMADO, "Cria tabela de Chamado")]
public class Versao0000004 : VersaoBase
{
    public override void Up()
    {
        CriarTabela(tabela: "Chamados", nomeId: "IdChamado")
            .WithColumn("Titulo").AsString(100).NotNullable()
            .WithColumn("Descricao").AsString(1000).NotNullable()
            .WithColumn("StatusChamado").AsInt32().NotNullable()
            .WithColumn("Prioridade").AsInt32().NotNullable()
            .WithColumn("AbertoEm").AsDateTime().NotNullable()
            .WithColumn("ResolvidoEm").AsDateTime().Nullable()
            .WithColumn("FechadoEm").AsDateTime().Nullable()
            .WithColumn("IdUsuarioComum").AsInt64().NotNullable().ForeignKey("Usuarios", "IdUsuario")
            .WithColumn("IdAgente").AsInt64().NotNullable().ForeignKey("Usuarios", "IdUsuario")
            .WithColumn("IdCategoria").AsInt64().NotNullable().ForeignKey("Categorias", "IdCategoria");
    }
}