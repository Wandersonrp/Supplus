using Microsoft.AspNetCore.Mvc;
using Supplus.Application.UseCases.Auth.Login;
using Supplus.Application.UseCases.Auth.RereshTokens;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Requests.RefreshTokens;
using Supplus.Domain;

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

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GerarRefreshTokenAsync(
        [FromServices] IGerarRefreshTokenUseCase useCase, 
        HttpContext httpContext, 
        CancellationToken token)
    {
        (string? ip, string userAgent) = ObterInformacoesDispositivo();

        var request = new RequestNovoTokenJson(ObterRefreshToken(httpContext), ip ?? string.Empty, userAgent);
        var resultado = await useCase.Executar(request, token);

        if(resultado.Falhou)
            return TratarFalha(resultado);

        httpContext.Response.Cookies.Append("refresh_token", resultado.Valor.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(RegrasConstants.EXPIRACAO_REFRESH_TOKEN_HORAS)
        });

        return Ok(resultado.Valor);
    }
}
