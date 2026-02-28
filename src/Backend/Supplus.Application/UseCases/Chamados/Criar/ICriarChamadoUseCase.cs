using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Comunicacao.Responses.Chamados;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Chamados.Criar;

public interface ICriarChamadoUseCase
{
    /// <summary>
    /// Cria um novo chamado a partir dos dados fornecidos,
    /// validando a categoria e persistindo no repositório.
    /// Retorna sucesso com os dados do chamado ou falha caso
    /// a categoria não seja encontrada.
    /// </summary>   
    Task<ResultadoPersonalizado<ResponseChamadoJson>> Executar(RequestCriarChamadoJson request, CancellationToken token);
}