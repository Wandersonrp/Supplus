using Supplus.Application.UseCases.Auth.Login;
using Supplus.Comunicacao.Requests.Auth;

namespace Supplus.UseCases.Tests.Auth;

public class LoginUseCaseTest
{
    [Fact]
    public void Login_Deve_Retornar_AccessToken_E_RefreshToken()
    {
        // Arrange
        var email = "admin@admin.com";
        var senha = "12345678";

        var request = new RequestLoginJson(email, senha);
        var sut = new LoginUseCase();

        // Act
        var resultado = sut.Executar(request);

        // Assert
        Assert.NotEmpty(resultado.Token.AccessToken);
        Assert.NotEmpty(resultado.Token.RefreshToken);
    }

    [Fact]
    public void Login_Deve_Lancar_Excecao_Quando_Senha_For_Vazia()
    {
        // Arrange
        var email = "admin@admin.com";
        var senha = "";

        var request = new RequestLoginJson(email, senha);
        var sut = new LoginUseCase();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => sut.Executar(request));        
    }
}
