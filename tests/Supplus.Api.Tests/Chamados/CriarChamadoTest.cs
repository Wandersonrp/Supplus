using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Api.Tests.Utils;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Infrastructure.Data.Context;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Supplus.Api.Tests.Chamados;

[Collection("Database collection")]
public class CriarChamadoTest
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _httpClient;
    private const string _rota = "/api/chamados";
    private UsuarioUtils _usuarioUtils;

    public CriarChamadoTest(ContainersFixture containersFixture)
    {
        _factory = new CustomWebApplicationFactory(containersFixture);
        _httpClient = _factory.CreateClient();

        _usuarioUtils = new UsuarioUtils();
    }

    [Fact]
    public async Task Deve_Criar_Chamado_Com_Sucesso()
    {
        // Arrange
        // Login
        var requestLogin = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        var responseLogin = await _usuarioUtils.AutenticarPorSenha(_httpClient, rota: "/api/auth/login", requestLogin);

        Assert.NotNull(responseLogin);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token.AccessToken);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SupplusDbContext>();

        var categoria = await dbContext.Categorias.FirstAsync();

        var titulo = "Instalação do Sistema nas novas máquinas.";
        var descricao = "Instalar o sistema nas 20 novas máquinas adquiridas.";
        var prioridade = Comunicacao.Enums.Prioridade.Alta;
        
        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, categoria.IdExterno);

        // Act
        var resultado = await _httpClient.PostAsJsonAsync(_rota, request);

        using var data = await resultado.Content.ReadAsStreamAsync();

        using var jsonDocument = JsonDocument.Parse(data);

        var rootElement = jsonDocument.RootElement;

        // Titulo
        var tituloData = rootElement.GetProperty("titulo").GetString();

        // Descricao
        var descrciaoData = rootElement.GetProperty("descricao").GetString();

        // Id
        var idData = rootElement.GetProperty("id").GetGuid();

        // Arrange
        Assert.Equal(HttpStatusCode.Created, resultado.StatusCode);
        Assert.IsType<Guid>(idData);
        Assert.NotNull(tituloData);        
    }

    [Fact]
    public async Task Nao_Deve_Criar_Chamado_Titulo_Vazio()
    {
        // Arrange
        // Login
        var requestLogin = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        var responseLogin = await _usuarioUtils.AutenticarPorSenha(_httpClient, rota: "/api/auth/login", requestLogin);

        Assert.NotNull(responseLogin);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token.AccessToken);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SupplusDbContext>();

        var categoria = await dbContext.Categorias.FirstAsync();

        var titulo = string.Empty;
        var descricao = "Instalar o sistema nas 20 novas máquinas adquiridas.";
        var prioridade = Comunicacao.Enums.Prioridade.Alta;

        var request = new RequestCriarChamadoJson(titulo, descricao, prioridade, categoria.IdExterno);

        // Act
        var resultado = await _httpClient.PostAsJsonAsync(_rota, request);

        using var data = await resultado.Content.ReadAsStreamAsync();

        using var jsonDocument = JsonDocument.Parse(data);

        var rootElement = jsonDocument.RootElement;

        var mensagensElement = rootElement.GetProperty("mensagens");

        var mensagens = mensagensElement
            .EnumerateArray()
            .Select(msg => msg.GetString())
            .ToList();

        // Arrange
        Assert.Equal(HttpStatusCode.BadRequest, resultado.StatusCode);
        var mensagemErro = Assert.Single(mensagens);
        Assert.Equal("O campo Titulo é obrigatório.", mensagemErro);
    }
}
