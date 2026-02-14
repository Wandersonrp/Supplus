using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supplus.Api.Attributes;
using Supplus.Api.Requirements;
using Supplus.Application.UseCases.Usuarios.Perfil;
using Supplus.Application.UseCases.Usuarios.Registrar;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Responses;
using Supplus.Comunicacao.Responses.Usuarios;

namespace Supplus.Api.Controllers.Usuarios;

public class UsuariosController : BaseController
{
    [HttpPost]
    [UsuarioAutenticado]
    [Authorize(Policies.PodeRegistrarUsuario)]
    [ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegistrarUsuario(
        [FromBody] RequestRegistrarUsuarioJson request, 
        [FromServices] IRegistrarUsuarioUseCase useCase, 
        CancellationToken token)
    {
        var resultado = await useCase.Executar(request, token);
        
        if (resultado.Falhou)
            return TratarFalha(resultado);

        return CreatedAtAction(nameof(RegistrarUsuario), resultado.Valor);
    }

    [HttpGet]
    [UsuarioAutenticado]
    [ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErroJson), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterPerfil([FromServices] IObterPerfilUseCase useCase, CancellationToken token)
    {
        var resultado = await useCase.Executar(token);
        return HandlerResponse(resultado, Ok);
    }
}
