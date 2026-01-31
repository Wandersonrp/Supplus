using Microsoft.AspNetCore.Mvc;
using Supplus.Comunicacao.Responses;
using Supplus.Exceptions;

namespace Supplus.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{ 
    protected IActionResult HandlerResponse<TResultado>(ResultadoPersonalizado<TResultado> resultado, Func<TResultado, IActionResult> objectResult)
    {
        if(resultado.Falhou)
            return TratarFalha(resultado);

        return objectResult(resultado.Valor);
    }

    protected IActionResult TratarFalha<TResultado>(ResultadoPersonalizado<TResultado> resultado)
    {
        return resultado.Erro.Codigo switch
        {
            nameof(CodigosErro.ErroDeValidacao) => BadRequest(new ResponseErroJson(resultado.Erro.Mensagens!)),
            nameof(CodigosErro.NaoEncontrado) => NotFound(new ResponseErroJson(resultado.Erro.Mensagem!)),
            nameof(CodigosErro.Conflito) => Conflict(new ResponseErroJson(resultado.Erro.Mensagem!)),
            nameof(CodigosErro.NaoAutorizado) or
                nameof(CodigosErro.CredencialInvalida) => Unauthorized(resultado?.Erro?.Mensagem is not null ?
                new ResponseErroJson(resultado.Erro.Mensagem!) : null),
            _ => throw new NotImplementedException("Código de erro não mapeado para resposta HTTP.")
        };
    }

    protected (string? ip, string userAgent) ObterInformacoesDispositivo()
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();        
        return (ip, userAgent);
    }

    protected string ObterRefreshToken()
    {
        var refreshToken = string.Empty;

        if (HttpContext.Request.Cookies.TryGetValue("refresh_token", out var resultado))
            refreshToken = resultado;

        return refreshToken;
    }
}
