using Microsoft.AspNetCore.Mvc;
using Supplus.Application.UseCases.Auth.Login;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Exceptions;

namespace Supplus.Api.Controllers.Auth;

public class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] RequestLoginJson request, [FromServices] ILoginUseCase useCase)
    {
        var resultado = await useCase.Executar(request);

        if(resultado.Falhou)
        {
            return resultado.Erro.Codigo switch
            {
                nameof(CodigosErro.ErroDeValidacao) => BadRequest(resultado.Erro),
                nameof(CodigosErro.CredencialInvalida) => Unauthorized(resultado.Erro),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro desconhecido.")
            };
        }

        return Ok(resultado.Valor);
    }
}
