using Supplus.Application.UseCases.Chamados.Criar;
using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;
using Supplus.Domain.Models;
using Supplus.Exceptions;
using UtilitariosCompartilhados.Tests.Builders.Repositories;
using UtilitariosCompartilhados.Tests.Builders.Services;

namespace Supplus.UseCases.Tests.Chamados.Criar;

public class CriarChamadoUseCaseTest
{
    [Fact]
    public async Task Usuario_Comum_Deve_Criar_Chamado_Com_Sucesso()
    {
        // Arrange
        var usuarioAutenticado = new UsuarioAutenticado(
            Id: 2, 
            Email: "johndoe@email.com", 
            Role.UsuarioComum, 
            PrimeiroNome: "John", 
            Sobrenome: "Doe", 
            IdExterno: Guid.NewGuid());

        // Cria os dados da requisição para criar um chamado
        var titulo = "Problema com o sistema";
        var descricao = "Estou enfrentando um problema ao acessar o sistema.";
        var prioridade = Comunicacao.Enums.Prioridade.Alta;        
        
        // Cria a categoria
        var categoria = new Categoria(criadoPor: 1, nome: "SistemaA", descricao: "Tecnologia");

        var idCategoria = categoria.IdExterno;

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, idCategoria);

        var sut = CriarUseCase(usuarioAutenticado, CancellationToken.None, categoria);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.ESucesso);
        Assert.Equal(titulo, resultado.Valor.Titulo);
        Assert.Equal(descricao, resultado.Valor.Descricao);
        Assert.Equal(prioridade, resultado.Valor.Prioridade);
        Assert.IsType<Guid>(resultado.Valor.Id);
    }

    [Fact]
    public async Task Deve_Falhar_Quando_Categoria_Nao_Existir()
    {
        // Arrange
        var usuarioAutenticado = new UsuarioAutenticado(
            Id: 2,
            Email: "johndoe@email.com",
            Role.UsuarioComum,
            PrimeiroNome: "John",
            Sobrenome: "Doe",
            IdExterno: Guid.NewGuid());

        // Cria os dados da requisição para criar um chamado
        var titulo = "Problema com o sistema";
        var descricao = "Estou enfrentando um problema ao acessar o sistema.";
        var prioridade = Comunicacao.Enums.Prioridade.Alta;

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, IdCategoria: Guid.NewGuid());

        var sut = CriarUseCase(usuarioAutenticado, CancellationToken.None);

        // Act  
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.Falhou);
        Assert.Equal(CodigosErro.NaoEncontrado, resultado.Erro.Codigo);
        Assert.NotNull(resultado.Erro.Mensagem);        
        Assert.Equal($"Categoria de id {request.IdCategoria} não encontrado(a).", resultado.Erro.Mensagem);
    }

    private CriarChamadoUseCase CriarUseCase(
        UsuarioAutenticado usuarioAutenticado, 
        CancellationToken token, 
        Categoria? categoria = null)
    {
        var usuarioAutenticadoService = new UsuarioAutenticadoBuilder();
        var chamadoRepository = new RepositoryBuilder<Chamado>();
        var categoriaRepository = new RepositoryBuilder<Categoria>();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(categoria is not null)
            categoriaRepository.ObterPorIdExternoAsync(categoria);

        usuarioAutenticadoService.ObterUsuarioAutenticadoAsync(usuarioAutenticado);

        return new CriarChamadoUseCase(
            usuarioAutenticadoService.Build(), 
            chamadoRepository.Build(), 
            categoriaRepository.Build(), 
            unitOfWork);
    }
}
