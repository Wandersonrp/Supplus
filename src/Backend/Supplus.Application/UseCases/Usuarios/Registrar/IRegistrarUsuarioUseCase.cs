using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Responses.Usuarios;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Usuarios.Registrar;

public interface IRegistrarUsuarioUseCase
{
    /// <summary>
    /// Processa o registro de um novo usuário no sistema, aplicando as regras de negócio de hierarquia
    /// e validações de integridade dos dados.
    /// </summary>
    /// <param name="request">Objeto contendo os dados necessários para o cadastro (E-mail, Nome, Senha e Tipo de Usuário).</param>
    /// <param name="token">Token de cancelamento para interromper a operação assíncrona caso necessário.</param>
    /// <returns>
    /// Retorna um <see cref="ResultadoPersonalizado{T}"/> contendo os dados do usuário recém-criado 
    /// ou uma lista de erros de validação/negócio.
    /// </returns>    
    Task<ResultadoPersonalizado<ResponseUsuarioJson>> Executar(RequestRegistrarUsuarioJson request, CancellationToken token);
}
