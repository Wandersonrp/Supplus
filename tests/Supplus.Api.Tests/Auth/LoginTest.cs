using Supplus.Comunicacao.Requests.Auth;
using Supplus.Exceptions;
using Supplus.Exceptions.Mensagens;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Supplus.Api.Tests.Auth;

[Collection("Database collection")]
public class LoginTest
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _httpClient;
    private const string _rota = "/api/auth/login";

    public LoginTest(ContainersFixture containersFixture)
    {
        _factory = new CustomWebApplicationFactory(containersFixture);
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_Deve_Retornar_AccessToken_E_RefreshToken()
    {
        // Arrange        
        var request = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        // Act
        var resultado = await _httpClient.PostAsJsonAsync(_rota, request);

        var data = await resultado.Content.ReadAsStringAsync();

        var jsonDocument = JsonDocument.Parse(data);

        var token = jsonDocument.RootElement.GetProperty("token");

        var accessToken = token.GetProperty("accessToken").GetString();
        var refreshToken = token.GetProperty("refreshToken").GetString();

        // Assert
        Assert.True(resultado.IsSuccessStatusCode);
        Assert.NotNull(accessToken);
        Assert.NotNull(refreshToken);
    }

    [Theory]
    [InlineData("admin@admin.com", "SenhaErrada123!")]
    [InlineData("suporte@supplus.com", "SenhaForte123!")]
    public async Task Login_Deve_Retornar_Erro_Credenciais_Invalidas(string email, string senha)
    {
        // Arrange        
        var request = new RequestLoginJson(Email: email, Senha: senha);

        // Act
        var resultado = await _httpClient.PostAsJsonAsync(_rota, request);

        var data = await resultado.Content.ReadAsStringAsync();

        var jsonDocument = JsonDocument.Parse(data);

        var mensagem = jsonDocument.RootElement.GetProperty("mensagem").GetString();        
        var codigo = jsonDocument.RootElement.GetProperty("codigo").GetString();        

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, resultado.StatusCode);
        Assert.Equal(MensagensErro.CREDENCIAIS_INVALIDAS, mensagem);        
        Assert.Equal(CodigosErro.CredencialInvalida, codigo);        
    }
}
