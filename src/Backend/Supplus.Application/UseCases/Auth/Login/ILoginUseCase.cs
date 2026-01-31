using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Responses.Auth;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task<ResultadoPersonalizado<ResponseLoginJson>> Executar(RequestLoginJson request, string? ip = null, string? dispositivoInfo = null);
}