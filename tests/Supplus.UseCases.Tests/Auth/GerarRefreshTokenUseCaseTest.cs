using Supplus.Application.UseCases.Auth.RereshTokens;
using Supplus.Comunicacao.Requests.RefreshTokens;
using Supplus.Domain.Entities;
using Supplus.Exceptions;
using UtilitariosCompartilhados.Tests.Builders.Repositories;
using UtilitariosCompartilhados.Tests.Builders.Services;

namespace Supplus.UseCases.Tests.Auth;

public class GerarRefreshTokenUseCaseTest
{
    [Fact]
    public async Task Deve_Gerar_Um_Novo_Refresh_Token()
    {
        // Arrange                 
        var dispositivoInfo = "TesteInfo";
        var ip = "127.0.0.1";

        var usuario = new Usuario(email: "johndoe@example.com", nome: "John", sobrenome: "Doe", 1);
        
        var refreshToken = new RefreshToken(idUsuario: usuario.Id, dispositivoInfo, ip);
        var refreshTokenFake = refreshToken.Token;

        var request = new RequestNovoTokenJson(refreshTokenFake, ip, dispositivoInfo);

        var sut = CriarUseCase(refreshToken, usuario);        

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.ESucesso);
        Assert.NotEmpty(resultado.Valor.AccessToken);
        Assert.NotEmpty(resultado.Valor.RefreshToken);
    }

    [Fact]
    public async Task Nao_Deve_Gerar_Refresh_Token_Quando_Refresh_Token_For_Invalido()
    {
        // Arrange              
        var dispositivoInfo = "TesteInfo";
        var ip = "127.0.0.1";

        var usuario = new Usuario(email: "johndoe@example.com", nome: "John", sobrenome: "Doe", 1);

        var refreshTokenFake = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var request = new RequestNovoTokenJson(refreshTokenFake, ip, dispositivoInfo);

        var sut = CriarUseCase(usuario: usuario);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.Falhou);
        Assert.NotNull(resultado.Erro.Mensagem);        
        Assert.Equal("Refresh Token inválido ou expirado.", resultado.Erro.Mensagem);
    }

    [Fact]
    public async Task Nao_Deve_Gerar_Refresh_Token_Quando_Token_Nao_Estiver_Ativo()
    {
        // Arrange
        var dispositivoInfo = "TesteInfo";
        var ip = "127.0.0.1";

        var usuario = new Usuario(email: "johndoe@example.com", nome: "John", sobrenome: "Doe", 1);

        var refreshToken = new RefreshToken(idUsuario: usuario.Id, dispositivoInfo, ip);
        var refreshTokenFake = refreshToken.Token;

        // Marca o token como usado para simular que não está mais ativo
        refreshToken.MarcarComoUsado();

        var request = new RequestNovoTokenJson(refreshTokenFake, ip, dispositivoInfo);

        var sut = CriarUseCase(refreshToken, usuario);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.Falhou);
        Assert.NotNull(resultado.Erro.Mensagem);
        Assert.Equal("Tentativa de reuso de sessão detectada.", resultado.Erro.Mensagem);
    }

    [Fact]
    public async Task Nao_Deve_Gerar_Refresh_Token_Quando_Usuario_Nao_Existir()
    {
        // Arrange
        var dispositivoInfo = "TesteInfo";
        var ip = "127.0.0.1";

        var refreshToken = new RefreshToken(idUsuario: 1, dispositivoInfo, ip);
        var refreshTokenFake = refreshToken.Token;

        var request = new RequestNovoTokenJson(refreshTokenFake, ip, dispositivoInfo);

        var sut = CriarUseCase(refreshToken);

        // Act
        var resultado = await sut.Executar(request, CancellationToken.None);

        // Assert
        Assert.True(resultado.Falhou);
        Assert.Equal(CodigosErro.NaoAutorizado, resultado.Erro.Codigo);
    }

    public static GerarRefreshTokenUseCase CriarUseCase(RefreshToken? refreshToken = null, Usuario? usuario = null)
    {
        var refreshTokenRepository = new RefreshTokenRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var usuarioRepository = new RepositoryBuilder<Usuario>();
        var geradorAccessToken = new GeradorTokenJwtBuilder();

        if (refreshToken is not null)
            refreshTokenRepository.ObterRefreshTokenPorTokenAsync(refreshToken);

        if(usuario is not null)
            usuarioRepository.ObterPorIdAsync(usuario);

        geradorAccessToken.Gerar();

        return new GerarRefreshTokenUseCase(
            refreshTokenRepository.Build(), 
            unitOfWork, 
            usuarioRepository.Build(), 
            geradorAccessToken.Build()); 
    }
}
