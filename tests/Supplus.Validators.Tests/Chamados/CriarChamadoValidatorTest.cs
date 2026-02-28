using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Comunicacao.Validators.Chamados;
using UtilitariosCompartilhados.Tests.Utils;

namespace Supplus.Validators.Tests.Chamados;

public class CriarChamadoValidatorTest
{
    [Fact]
    public void Deve_Validar_Criar_Chamado_Corretamente()
    {
        // Arrange
        var validator = new CriarChamadoValidator();

        var titulo = "Instalação do sistema.";
        var descricao = "Instalar o sistemas nos computadores.";
        var prioridade = Comunicacao.Enums.Prioridade.Baixa;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Nao_Deve_Validar_Criar_Chamado_Titulo_Vazio()
    {
        // Arrange
        var validator = new CriarChamadoValidator();

        var titulo = string.Empty;
        var descricao = "Instalar o sistemas nos computadores.";
        var prioridade = Comunicacao.Enums.Prioridade.Baixa;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        var erro = Assert.Single(result.Errors);
        Assert.Equal("O campo Titulo é obrigatório.", erro.ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Criar_Chamado_Titulo_Tamanho_Maximo()
    {
        // Arrange
        var validator = new CriarChamadoValidator();                

        var titulo = StringUtils.CriarPalavra(101);        

        var descricao = "Instalar o sistemas nos computadores.";
        var prioridade = Comunicacao.Enums.Prioridade.Baixa;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        var erro = Assert.Single(result.Errors);
        Assert.Equal("O campo Titulo deve ter no máximo 100 caracteres.", erro.ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Criar_Chamado_Descricao_Vazio()
    {
        // Arrange
        var validator = new CriarChamadoValidator();

        var titulo = "Instalação do sistema.";
        var descricao = string.Empty;
        var prioridade = Comunicacao.Enums.Prioridade.Baixa;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        var erro = Assert.Single(result.Errors);
        Assert.Equal("O campo Descricao é obrigatório.", erro.ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Criar_Chamado_Descricao_Tamanho_Maximo()
    {
        // Arrange
        var validator = new CriarChamadoValidator();

        var titulo = "Instalação do sistema.";

        var descricao = StringUtils.CriarPalavra(1001);
        var prioridade = Comunicacao.Enums.Prioridade.Baixa;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        var erro = Assert.Single(result.Errors);
        Assert.Equal("O campo Descricao deve ter no máximo 1000 caracteres.", erro.ErrorMessage);
    }

    [Fact]
    public void Nao_Deve_Validar_Criar_Chamado_Prioridade_Invalida()
    {
        // Arrange
        var validator = new CriarChamadoValidator();

        var titulo = "Instalação do sistema.";
        var descricao = "Instalar o sistemas nos computadores.";
        var prioridade = (Comunicacao.Enums.Prioridade)10;
        var idCategoria = Guid.NewGuid();

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        // Act
        var result = validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.NotNull(result.Errors);
        var erro = Assert.Single(result.Errors);
        Assert.Equal("Valor do(a) Prioridade inválido(a).", erro.ErrorMessage);
    }    
}
