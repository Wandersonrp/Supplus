using Supplus.Comunicacao.Responses.Usuarios;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Usuarios.Perfil;

public interface IObterPerfilUseCase
{
    Task<ResultadoPersonalizado<ResponseUsuarioJson>> Executar(CancellationToken token);
}