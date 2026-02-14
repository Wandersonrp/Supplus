using Supplus.Comunicacao.Requests.Auth;
using System.Text.Json;

namespace Supplus.Api.Tests.Usuarios;

[Collection("Database collection")]
public class ObterPerfilTest
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _httpClient;
    private const string _rota = "/api/usuarios";

    public ObterPerfilTest(ContainersFixture containersFixture)
    {
        _factory = new CustomWebApplicationFactory(containersFixture);
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task Deve_Obter_Perfil_Com_Sucesso_Retorna_Perfil_Usuario()
    {
        // Arrange
        var requestLogin = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        // Act
        var resultado = await _httpClient.GetAsync(_rota);

        Assert.True(resultado.IsSuccessStatusCode);

        using var stream = await resultado.Content.ReadAsStreamAsync();
        var jsonDocument = await JsonDocument.ParseAsync(stream);

        var id = jsonDocument.RootElement.GetProperty("id").GetGuid();
        var email = jsonDocument.RootElement.GetProperty("email").GetString();
        var nomeCompleto = jsonDocument.RootElement.GetProperty("nome").GetString();

        // Assert
        Assert.IsType<Guid>(id);
        Assert.Equal(requestLogin.Email, email);
        Assert.Equal("Admin Admin", nomeCompleto);
    }
}
