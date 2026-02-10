using Microsoft.AspNetCore.Mvc;
using Supplus.Api.Attributes;
using Supplus.Application.UseCases.Usuarios.Registrar;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Responses.Usuarios;

namespace Supplus.Api.Controllers.Usuarios;

public class UsuariosController : BaseController
{
    [HttpPost]
    [UsuarioAutenticado]
    // TODO: ADICIONAR A POLICY DE AUTORIZAÇÃO PARA PERMITIR APENAS USUÁRIOS COM ROLE DE ADMINISTRADOR E SUPORTE
    [ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegistrarUsuario(
        [FromBody] RequestRegistrarUsuarioJson request, 
        [FromServices] IRegistrarUsuarioUseCase useCase)
    {
        var resultado = await useCase.Executar(request, CancellationToken.None);
        
        if (resultado.Falhou)
            return TratarFalha(resultado);

        return CreatedAtAction(nameof(RegistrarUsuario), resultado.Valor);
    }
}
