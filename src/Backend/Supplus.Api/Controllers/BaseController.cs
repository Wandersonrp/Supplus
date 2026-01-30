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
        {
            return resultado.Erro.Codigo switch
            {
                nameof(CodigosErro.ErroDeValidacao) => BadRequest(new ResponseErroJson(resultado.Erro.Mensagens!)),
                nameof(CodigosErro.NaoEncontrado) => NotFound(new ResponseErroJson(resultado.Erro.Mensagem!)),
                nameof(CodigosErro.Conflito) => Conflict(new ResponseErroJson(resultado.Erro.Mensagem!)),
                nameof(CodigosErro.NaoAutorizado) or 
                    nameof(CodigosErro.CredencialInvalida) => Unauthorized(new ResponseErroJson(resultado.Erro.Mensagem!)),                
                _ => throw new NotImplementedException("Código de erro não mapeado para resposta HTTP.")
            };
        }

        return objectResult(resultado.Valor);
    }
}
