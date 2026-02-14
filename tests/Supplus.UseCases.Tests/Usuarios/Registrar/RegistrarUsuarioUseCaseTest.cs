using Supplus.Application.UseCases.Usuarios.Registrar;
using Supplus.Comunicacao.Enums;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Domain.Entities;
using Supplus.Domain.Models;
using Supplus.Exceptions.Mensagens;
using UtilitariosCompartilhados.Tests.Builders.Repositories;
using UtilitariosCompartilhados.Tests.Builders.Services;

namespace Supplus.UseCases.Tests.Usuarios.Registrar;

public class RegistrarUsuarioUseCaseTest
{
    [Fact]
    public async Task Deve_Registrar_Usuario_Com_Sucesso()
    {
        // Arrange
        var primeiroNome = "John";  
        var sobrenome = "Doe";
        var email = "johndoe@example.com";
        var role = Role.UsuarioComum;

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Usuario Admin
        var usuarioAuenticado = new UsuarioAutenticado(
            Id: 1, 
            Email: "admin@admin.com", 
            Role: Domain.Enums.Role.Administrador, 
            PrimeiroNome: "Admin", 
            Sobrenome: "Admin", 
            IdExterno: Guid.NewGuid());

        var sut = CriarUseCase(usuarioAuenticado, CancellationToken.None);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert                    
        Assert.True(resultado.ESucesso);
        Assert.NotNull(resultado.Valor);
        Assert.IsType<Guid>(resultado.Valor.Id);
        Assert.Equal(primeiroNome, resultado.Valor.PrimeiroNome);
        Assert.Equal(sobrenome, resultado.Valor.Sobrenome);
        Assert.Equal(email, resultado.Valor.Email);
        Assert.Equal(role, resultado.Valor.Role);
    }

    [Fact(DisplayName = "Não deve ser possível registrar usuário com e-mail que já existe.")]
    public async Task Nao_Deve_Registrar_Usuario_Com_Email_Existente()
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";
        var email = "johndoe@example.com";
        var role = Role.UsuarioComum;

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Usuario Admin
        var usuarioAuenticado = new UsuarioAutenticado(
            Id: 1, 
            Email: "admin@admin.com", 
            Role: Domain.Enums.Role.Administrador, 
            PrimeiroNome: "Admin", 
            Sobrenome: "Admin", 
            IdExterno: Guid.NewGuid());

        // Usuario que já existe no banco
        var usuarioExistente = new Usuario(email, primeiroNome, sobrenome, usuarioAuenticado.Id);

        var sut = CriarUseCase(usuarioAuenticado, CancellationToken.None, usuarioExistente);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert                    
        Assert.True(resultado.Falhou);
        Assert.NotNull(resultado.Erro.Mensagem);
        Assert.Equal(String.Format(MensagensErro.CONFLITO, "Usuário"), resultado.Erro.Mensagem);        
    }

    [Theory(DisplayName = "Agente de Suporte não deve poder registrar usuários do tipo Administrador e Suporte.")]
    [InlineData(Role.AgenteSuporte)]
    [InlineData(Role.Administrador)]
    public async Task Suporte_Nao_Deve_Poder_Registrar_Usuario_Admin_E_Suporte(Role role)
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";
        var email = "johndoe@example.com";        

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, role);

        // Usuario Suporte
        var usuarioAuenticado = new UsuarioAutenticado(
            Id: 2, 
            Email: "suporte@suporte.com", 
            Role: Domain.Enums.Role.AgenteSuporte, 
            PrimeiroNome: "Suporte", 
            Sobrenome: "Suporte", 
            IdExterno: Guid.NewGuid());        

        var sut = CriarUseCase(usuarioAuenticado, CancellationToken.None);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert                    
        Assert.True(resultado.Falhou);
        Assert.NotNull(resultado.Erro.Mensagem);
        Assert.Equal(MensagensErro.PERMISSOES_INVALIDAS, resultado.Erro.Mensagem);
    }   

    public static RegistrarUsuarioUseCase CriarUseCase(
        UsuarioAutenticado usuarioAutenticado, 
        CancellationToken token, 
        Usuario? usuario = null)
    {
        var usuarioRepository = new UsuarioRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var usuarioAutenticadoService = new UsuarioAutenticadoBuilder();
        var emailService = new EmailServiceBuilder().Build();

        if (usuario is not null)
            usuarioRepository.ExisteUsuarioComEmailAsync(usuario.Email, token);

        usuarioAutenticadoService.ObterUsuarioAutenticadoAsync(usuarioAutenticado);

        return new RegistrarUsuarioUseCase(
            usuarioRepository.Build(), 
            unitOfWork, 
            usuarioAutenticadoService.Build(), 
            emailService);
    }
}
