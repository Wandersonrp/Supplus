using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.ALTERAR_FKS_NULLABLE_CATEGORIA, "Altera as colunas IdUsuarioComum e IdAgente de NotNull para Nullable")]
public class Versao0000006 : ForwardOnlyMigration
{
    public override void Up()
    {
        Alter.Table("Chamados").AlterColumn("IdUsuarioComum").AsInt64().Nullable();
        Alter.Table("Chamados").AlterColumn("IdAgente").AsInt64().Nullable();
    }
}
