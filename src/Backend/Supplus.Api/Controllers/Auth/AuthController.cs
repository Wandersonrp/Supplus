using Microsoft.AspNetCore.Mvc;
using Supplus.Application.UseCases.Auth.Login;
using Supplus.Comunicacao.Requests.Auth;

namespace Supplus.Api.Controllers.Auth;

public class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] RequestLoginJson request, [FromServices] ILoginUseCase useCase)
    {
        (string? ip, string userAgent) = ObterInformacoesDispositivo();

        var resultado = await useCase.Executar(request, ip, userAgent);
        return HandlerResponse(resultado, Ok);                
    }
}
