using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

public abstract class VersaoBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CriarTabela(string tabela, string nomeId)
    {
        return Create.Table(tabela)
            .WithColumn(nomeId).AsInt64().PrimaryKey().Identity()
            .WithColumn("IdExterno").AsGuid().NotNullable().Indexed()
            .WithColumn("CriadoEm").AsDateTime().NotNullable()
            .WithColumn("CriadoPor").AsInt64().Nullable()
            .WithColumn("AtualizadoEm").AsDateTime().Nullable()
            .WithColumn("DeletadoEm").AsDateTime().Nullable()
            .WithColumn("DeletadoPor").AsInt64().Nullable()
            .WithColumn("MotivoDelecao").AsString(300).Nullable()
            .WithColumn("Status").AsInt32().NotNullable();
    }
}
