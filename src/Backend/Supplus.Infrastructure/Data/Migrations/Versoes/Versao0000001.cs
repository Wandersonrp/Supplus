using FluentMigrator;

namespace Supplus.Infrastructure.Data.Migrations.Versoes;

[Migration(VersoesMigration.CRIAR_TABELA_USUARIO, "Cria tabela de Usuario")]
public class Versao0000001 : VersaoBase
{
    public override void Up()
    {
        CriarTabela(tabela: "Usuarios", nomeId: "IdUsuario")
            .WithColumn("Nome").AsString(50).NotNullable()
            .WithColumn("Sobrenome").AsString(50).NotNullable()
            .WithColumn("Email").AsString(150).NotNullable().Indexed()            
            .WithColumn("Senha").AsString().NotNullable()
            .WithColumn("Role").AsInt32().NotNullable();
    }
}
