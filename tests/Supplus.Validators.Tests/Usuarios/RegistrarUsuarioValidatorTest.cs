using Supplus.Comunicacao.Enums;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Validators.Usuarios;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Validators.Tests.Usuarios;

public class RegistrarUsuarioValidatorTest
{
    [Theory]
    [InlineData("John", "Doe", "johndoe@example.com", Role.UsuarioComum)]
    [InlineData("John", "Doe", "johndoe@example.com", Role.AgenteSuporte)]
    [InlineData("John", "Doe", "johndoe@example.com", Role.Administrador)]
    public void Deve_Validar_Dados_Registro_Usuario_Corretamente(string primeiroNome, string sobrenome, string email, Role role)
    {
        // Arrange
        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void Nao_Deve_Validar_Dados_Registro_Primeiro_Nome_Vazio()
    {
        // Arrange
        var primeiroNome = string.Empty;
        var sobrenome = "Doe";
        var email = "johndoe@example.com";
        var role = Role.UsuarioComum;

        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestRegistrarUsuarioJson.PrimeiroNome)), resultado.Errors.First().ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Dados_Registro_Primeiro_Nome_Maximo_Caracteres()
    {
        // Arrange
        var primeiroNome = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var sobrenome = "Doe";
        var email = "johndoe@example.com";
        var role = Role.UsuarioComum;

        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestRegistrarUsuarioJson.PrimeiroNome), 50), resultado.Errors.First().ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Dados_Registro_Email_Vazio()
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";
        var email = string.Empty;
        var role = Role.UsuarioComum;

        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestRegistrarUsuarioJson.Email)), resultado.Errors.First().ErrorMessage);
    }

    [Theory]
    [InlineData("johndoe")]
    [InlineData("johndoe@")]
    [InlineData("@com.br")]
    [InlineData("@example.com.br")]
    public void Nao_Deve_Validar_Dados_Registro_Email_Invalido(string email)
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";        
        var role = Role.UsuarioComum;

        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(MensagensErro.EMAIL_INVALIDO, resultado.Errors.First().ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Dados_Registro_Role_Invalida()
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";
        var email = "johndoe@example.com";
        var role = (Role)8;

        var validator = new RegistrarUsuarioValidator();

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Act
        var resultado = validator.Validate(request);

        // Assert
        Assert.False(resultado.IsValid);
        Assert.Single(resultado.Errors);
        Assert.Equal(String.Format(MensagensErro.VALOR_INVALIDO, nameof(RequestRegistrarUsuarioJson.Role)), resultado.Errors.First().ErrorMessage);
    }
}
