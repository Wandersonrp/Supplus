using Supplus.Application.UseCases.Auth.Login;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;
using Supplus.Exceptions.Mensagens;
using UtilitariosCompartilhados.Tests.Builders.Repositories;
using UtilitariosCompartilhados.Tests.Builders.Services;

namespace Supplus.UseCases.Tests.Auth;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Login_Deve_Retornar_AccessToken_E_RefreshToken()
    {
        // Arrange
        var email = "admin@admin.com";
        var senha = "12345678";

        var request = new RequestLoginJson(email, senha);        

        var usuario = new Usuario(request.Email!, nome: "John", sobrenome: "Doe", role: Role.Administrador);

        // Atribui a senha hasheada ao usuário
        usuario.AtribuirSenha(PasswordHasherBuilder.Build().HashPassword(usuario, request.Senha!));

        var sut = CriarUseCase(usuario);

        // Act
        var resultado = await sut.Executar(request);

        // Assert
        Assert.True(resultado.ESucesso);
        Assert.NotEmpty(resultado.Valor.Token.AccessToken);
        Assert.NotEmpty(resultado.Valor.Token.RefreshToken);
    }

    [Fact]
    public async Task Login_Deve_Retornar_Erro_Quando_Senha_For_Vazia()
    {
        // Arrange
        var email = "admin@admin.com";
        var senha = "";

        var request = new RequestLoginJson(email, senha);
        var sut = CriarUseCase();

        // Act 
        var resultado = await sut.Executar(request);

        // Assert
        Assert.False(resultado.ESucesso);
        Assert.NotNull(resultado.Erro.Mensagens);
        Assert.Single(resultado.Erro.Mensagens);
        Assert.Collection(resultado.Erro.Mensagens, msg =>
        {
            Assert.Equal(string.Format(MensagensErro.CAMPO_OBRIGATORIO, "Senha"), msg);
        });
    }


    public static LoginUseCase CriarUseCase(Usuario? usuario = null)
    {
        var usuarioRepository = new UsuarioRepositoryBuilder();
        var passwordHasher = PasswordHasherBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if (usuario is not null)
            usuarioRepository.ObterPorEmailAsync(usuario);

        return new LoginUseCase(usuarioRepository.Build(), passwordHasher, unitOfWork);
    }
}
