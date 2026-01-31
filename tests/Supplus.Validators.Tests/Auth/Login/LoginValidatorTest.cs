using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Validators.Auth.Login;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Validators.Tests.Auth.Login;

public class LoginValidatorTest
{
    [Theory]
    [InlineData("admin@admin.com", "12345678")]
    [InlineData("suporte@supplus.com", "029472198")]
    public void Deve_Validar_Login_Corretamente(string email, string senha)
    {
        // Arrange
        var validator = new LoginValidator();

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.True(resultado.IsValid);
    }

    [Theory]
    [InlineData("@admin.com")]    
    [InlineData("admin.com")]
    public void Deve_Invalidar_Login_Email_Invalido(string email)
    {
        // Arrange
        var validator = new LoginValidator();

        var senha = "12345678";

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensErro.EMAIL_INVALIDO, resultado.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Deve_Invalidar_Login_Email_Vazio()
    {
        // Arrange
        var validator = new LoginValidator();

        var senha = "12345678";
        var email = "";

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.False(resultado.IsValid);
        var validationFailure = Assert.Single(resultado.Errors);
        Assert.Equal(string.Format(MensagensErro.CAMPO_OBRIGATORIO, "Email"), validationFailure.ErrorMessage);
    }

    [Fact]
    public void Deve_Invalidar_Login_Senha_Vazia()
    {
        // Arrange
        var validator = new LoginValidator();

        var senha = "";
        var email = "johndoe@supplus.com";

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.False(resultado.IsValid);
        var validationFailure = Assert.Single(resultado.Errors);
        Assert.Equal(string.Format(MensagensErro.CAMPO_OBRIGATORIO, "Senha"), validationFailure.ErrorMessage);
    }

    [Fact]
    public void Deve_Invalidar_Login_Senha_Tamanho_Excede_Maximo()
    {
        // Arrange
        var validator = new LoginValidator();

        var senha = "12345678901234567890123";
        var email = "johndoe@supplus.com";

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.False(resultado.IsValid);
        var validationFailure = Assert.Single(resultado.Errors);
        Assert.Equal(string.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, "Senha", 20), validationFailure.ErrorMessage);
    }

    [Fact]
    public void Deve_Invalidar_Login_Senha_Tamanho_Minimo()
    {
        // Arrange
        var validator = new LoginValidator();

        var senha = "1234";
        var email = "johndoe@supplus.com";

        var valido = new RequestLoginJson(email, senha);

        // Act
        var resultado = validator.Validate(valido);

        // Assert
        Assert.False(resultado.IsValid);
        var validationFailure = Assert.Single(resultado.Errors);
        Assert.Equal(string.Format(MensagensErro.TAMANHO_MINIMO_CAMPO, "Senha", 8), validationFailure.ErrorMessage);
    }
}
