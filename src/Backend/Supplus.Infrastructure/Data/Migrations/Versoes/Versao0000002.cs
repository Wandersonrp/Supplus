using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.CRIAR_TABELA_REFRESH_TOKEN, "Cria tabela de Usuario")]
public class Versao0000002 : VersaoBase
{
    public override void Up()
    {
        CriarTabela(tabela: "RefreshTokens", nomeId: "IdRefreshToken")
            .WithColumn("Token").AsString().NotNullable()
            .WithColumn("DispositivoInfo").AsString().NotNullable()
            .WithColumn("Ip").AsString().Nullable()
            .WithColumn("DataExpiracao").AsDateTime().NotNullable()
            .WithColumn("FoiUsado").AsBoolean().NotNullable()
            .WithColumn("FoiRevogado").AsBoolean().NotNullable()
            .WithColumn("IdUsuario").AsInt64().NotNullable().ForeignKey("Usuarios", "IdUsuario");        
    }
}