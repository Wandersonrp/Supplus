using Supplus.Api.Tests.Utils;
using Supplus.Comunicacao.Requests.Auth;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Supplus.Api.Tests.Usuarios;

[Collection("Database collection")]
public class ObterPerfilTest
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _httpClient;
    private const string _rota = "/api/usuarios";
    private UsuarioUtils _usuarioUtils;


    public ObterPerfilTest(ContainersFixture containersFixture)
    {
        _factory = new CustomWebApplicationFactory(containersFixture);
        _httpClient = _factory.CreateClient();
        _usuarioUtils = new UsuarioUtils();
    }

    [Fact]
    public async Task Deve_Obter_Perfil_Com_Sucesso_Retorna_Perfil_Usuario()
    {
        // Arrange
        var requestLogin = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        // Realiza o login para obter o token de autenticação
        var responseLogin = await _usuarioUtils.AutenticarPorSenha(_httpClient, rota: "/api/auth/login", requestLogin);

        Assert.NotNull(responseLogin);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token.AccessToken);

        // Act
        var resultado = await _httpClient.GetAsync(_rota);

        Assert.True(resultado.IsSuccessStatusCode);

        using var stream = await resultado.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(stream);

        var id = jsonDocument.RootElement.GetProperty("id").GetGuid();
        var email = jsonDocument.RootElement.GetProperty("email").GetString();
        var nomeCompleto = jsonDocument.RootElement.GetProperty("nomeCompleto").GetString();

        // Assert
        Assert.IsType<Guid>(id);
        Assert.Equal(requestLogin.Email, email);
        Assert.Equal("Admin Admin", nomeCompleto);
    }
}
