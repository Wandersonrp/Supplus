namespace Supplus.UseCases.Tests.Usuarios.Registrar;

public class RegistrarUsuarioUseCaseTest
{
    [Fact]
    public void Deve_Registrar_Usuario_Com_Sucesso()
    {
        // Arrange
        var primeiroNome = "John";  
        var sobrenome = "Doe";
        var email = "johndoe@example.com";

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email);

        // Act
        var sut = new RegistrarUsuarioUseCase(request);

        // Assert
        Assert.True(sut.Sucesso);
        Assert.NotNull(sut.Valor);
        Assert.IsType<Guid>(sut.Valor.Id);
    }
}
