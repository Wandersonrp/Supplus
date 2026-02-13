using Microsoft.AspNetCore.Http;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Responses.Auth;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Supplus.Api.Tests.Usuarios;

[Collection("Database collection")]
public class RegistrarUsuarioTest
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _httpClient;
    private const string _rota = "/api/usuarios";

    public RegistrarUsuarioTest(ContainersFixture containersFixture)
    {
        _factory = new CustomWebApplicationFactory(containersFixture);
        _httpClient = _factory.CreateClient();
    }

    [Fact(DisplayName = "Deve registrar usuário com sucesso.")]
    public async Task Deve_Registrar_Usuario_Com_Sucesso_Retorna_Usuario_Criado()
    {
        // Arrange
        var primeiroNome = "John";
        var sobrenome = "Doe";
        var email = "johndoe@example.com";        

        // Autentica usuário administrador para obter token de autenticação
        var requestLogin = new RequestLoginJson(Email: "admin@admin.com", Senha: "SenhaForte123!");

        var resultadoLogin = await _httpClient.PostAsJsonAsync("/api/auth/login", requestLogin);
        var responseLogin = await resultadoLogin.Content.ReadFromJsonAsync<ResponseLoginJson>();

        Assert.NotNull(responseLogin?.Token);

        var request = new RequestRegistrarUsuarioJson(primeiroNome, sobrenome, email, Comunicacao.Enums.Role.UsuarioComum);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseLogin.Token.AccessToken);

        // Act
        var resultado = await _httpClient.PostAsJsonAsync(_rota, request);
        
        Assert.True(resultado.IsSuccessStatusCode);

        // Verfica se o status code é Created (201)
        Assert.Equal(HttpStatusCode.Created, resultado.StatusCode);        
    }
}
