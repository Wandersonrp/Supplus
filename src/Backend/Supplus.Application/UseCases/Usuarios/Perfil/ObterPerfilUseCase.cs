using Supplus.Comunicacao.Responses.Usuarios;
using Supplus.Domain.Services;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Usuarios.Perfil;

public class ObterPerfilUseCase : IObterPerfilUseCase
{
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ObterPerfilUseCase(IUsuarioAutenticado usuarioAutenticado)
    {
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ResultadoPersonalizado<ResponseUsuarioJson>> Executar(CancellationToken token)
    {
        var usuarioAutenticado = await _usuarioAutenticado.ObterUsuarioAutenticadoAsync(token);

        var responseUsuarioJson = new ResponseUsuarioJson(
            usuarioAutenticado.IdExterno, 
            usuarioAutenticado.PrimeiroNome, 
            usuarioAutenticado.Sobrenome, 
            usuarioAutenticado.Email, 
            (Comunicacao.Enums.Role) usuarioAutenticado.Role, 
            usuarioAutenticado.ObterNomeCompleto());

        return ResultadoPersonalizado<ResponseUsuarioJson>.Sucesso(responseUsuarioJson);
    }
}
