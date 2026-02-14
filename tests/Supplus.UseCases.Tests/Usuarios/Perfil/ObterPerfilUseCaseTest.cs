using Supplus.Application.UseCases.Usuarios.Perfil;
using Supplus.Domain.Models;
using UtilitariosCompartilhados.Tests.Builders.Services;

namespace Supplus.UseCases.Tests.Usuarios.Perfil;

public class ObterPerfilUseCaseTest
{
    [Fact(DisplayName = "Deve obter o perfil do usuário com sucesso.")]
    public async Task Deve_Obter_Perfil_Com_Sucesso()
    {
        // Arrange

        var usuarioAuenticado = new UsuarioAutenticado(
            Id: 1,
            Email: "admin@admin.com",
            Role: Domain.Enums.Role.Administrador,
            PrimeiroNome: "Admin",
            Sobrenome: "Admin",
            IdExterno: Guid.NewGuid());

        var sut = CriarUseCase(usuarioAuenticado);

        // Act
        var resultado = await sut.Executar(CancellationToken.None);

        // Assert
        Assert.NotNull(resultado.Valor);
        Assert.Equal(usuarioAuenticado.Email, resultado.Valor.Email);
        Assert.IsType<Guid>(resultado.Valor.Id);
    }

    public static ObterPerfilUseCase CriarUseCase(UsuarioAutenticado? usuarioAutenticado = null)
    {
        var usuarioAutenticadoService = new UsuarioAutenticadoBuilder();

        if (usuarioAutenticado is not null)
            usuarioAutenticadoService.ObterUsuarioAutenticadoAsync(usuarioAutenticado);

        return new ObterPerfilUseCase(usuarioAutenticadoService.Build());
    }
}
