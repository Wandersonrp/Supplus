using Microsoft.AspNetCore.Mvc;
using Supplus.Api.Attributes;
using Supplus.Application.UseCases.Chamados.Criar;
using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Comunicacao.Responses;
using Supplus.Comunicacao.Responses.Chamados;

namespace Supplus.Api.Controllers.Chamados;

public class ChamadosController : BaseController
{
    [HttpPost]
    [UsuarioAutenticado]
    [ProducesResponseType(typeof(ResponseChamadoJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CriarAsync(
        [FromBody] RequestCriarChamadoJson request, 
        [FromServices] ICriarChamadoUseCase useCase, 
        CancellationToken token)
    {
        var resultado = await useCase.Executar(request, token);

        if (resultado.Falhou)
            return TratarFalha(resultado);

        return Created(nameof(CriarAsync), resultado.Valor);
    }
}