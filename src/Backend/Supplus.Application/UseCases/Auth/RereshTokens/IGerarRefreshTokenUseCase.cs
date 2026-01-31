using Supplus.Comunicacao.Requests.RefreshTokens;
using Supplus.Comunicacao.Responses.Auth;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Auth.RereshTokens;

public interface IGerarRefreshTokenUseCase
{
    Task<ResultadoPersonalizado<ResponseTokenJson>> Executar(RequestNovoTokenJson request, CancellationToken token);
}