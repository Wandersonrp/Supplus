using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Responses.Auth;
using System.Net.Http.Json;

namespace Supplus.Api.Tests.Utils;

public class UsuarioUtils
{
    public async Task<ResponseLoginJson?> AutenticarPorSenha(HttpClient httpClient, string rota, RequestLoginJson request)
    {
        var resposta = await httpClient.PostAsJsonAsync(rota, request);
        resposta.EnsureSuccessStatusCode();
        return await resposta.Content.ReadFromJsonAsync<ResponseLoginJson>();
    }
}
