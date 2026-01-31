using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Domain.Entities;
using Supplus.Infrastructure.Data.Context;
using System.Net;
using System.Text.Json;

namespace Supplus.Api.Tests.Auth
{
    [Collection("Database collection")]
    public class GerarRefreshTokenTest
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _httpClient;
        private const string _rota = "/api/auth/refresh-token";

        public GerarRefreshTokenTest(ContainersFixture containersFixture)
        {
            _factory = new CustomWebApplicationFactory(containersFixture);
            _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Deve_Retornar_Novos_Tokens_E_SetCookie()
        {
            // Arrange: cria um refresh token válido no banco para o usuário seedado (admin@admin.com)
            string refreshTokenExistente;
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<SupplusDbContext>();
                var usuario = db.Usuarios.Single(u => u.Email == "admin@admin.com");

                var refreshTokenEntidade = new RefreshToken(usuario.Id, dispositivoInfo: "IntegrationTestDevice", ip: "127.0.0.1");
                db.RefreshTokens.Add(refreshTokenEntidade);
                await db.SaveChangesAsync();

                refreshTokenExistente = refreshTokenEntidade.Token;
            }

            // Prepara a requisição com cookie "refresh_token"
            var request = new HttpRequestMessage(HttpMethod.Post, _rota);
            request.Headers.Add("Cookie", $"refresh_token={refreshTokenExistente}");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert - status
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert - body contém tokens
            var content = await response.Content.ReadAsStreamAsync();
            using var jsonDocument = JsonDocument.Parse(content);
            var root = jsonDocument.RootElement;

            var accessToken = root.GetProperty("accessToken").GetString();
            var refreshToken = root.GetProperty("refreshToken").GetString();
            
            Assert.False(string.IsNullOrWhiteSpace(accessToken));
            Assert.False(string.IsNullOrWhiteSpace(refreshToken));

            // novo token deve ser diferente
            Assert.NotEqual(refreshTokenExistente, refreshToken); 

            // Assert - Set-Cookie header presente com refresh_token
            Assert.True(response.Headers.TryGetValues("Set-Cookie", out var setCookieValues));
            var setCookie = setCookieValues.FirstOrDefault();
            Assert.NotNull(setCookie);
            Assert.Contains("refresh_token=", setCookie);
            Assert.Contains("HttpOnly", setCookie, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Secure", setCookie, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("SameSite=Strict", setCookie, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task Deve_Retornar_401_Quando_RefreshToken_Invalido()
        {
            // Arrange: cookie com token inexistente/aleatório
            var fakeToken = Guid.NewGuid().ToString("N");
            var request = new HttpRequestMessage(HttpMethod.Post, _rota);
            request.Headers.Add("Cookie", $"refresh_token={fakeToken}");

            // Act
            var response = await _httpClient.SendAsync(request);

            // Assert - deve ser 401 Unauthorized
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            // Assert - corpo com mensagens de erro padronizadas (mensagens em array)
            using var stream = await response.Content.ReadAsStreamAsync();
            using var jsonDocument = JsonDocument.Parse(stream);
            var root = jsonDocument.RootElement;
            
            if (jsonDocument.RootElement.TryGetProperty("mensagens", out var mensagensElement))
            {
                var mensagens = mensagensElement.EnumerateArray().Select(e => e.GetString()).ToList();
                Assert.Single(mensagens);
                Assert.Contains("Refresh Token inválido", mensagens[0], StringComparison.OrdinalIgnoreCase);
            }
            else if (root.TryGetProperty("mensagem", out var mensagemElement))
            {
                var mensagem = mensagemElement.GetString();
                Assert.NotNull(mensagem);
                Assert.Contains("Refresh Token inválido", mensagem, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                // fallback para validar que o corpo não está vazio
                var texto = await response.Content.ReadAsStringAsync();
                Assert.False(string.IsNullOrWhiteSpace(texto));
            }
        }
    }
}

