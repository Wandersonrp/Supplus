using Supplus.Domain.Models;

namespace Supplus.Domain.Services;

/// <summary>
/// Define o contrato para serviços que obtêm informações do usuário autenticado.
/// </summary>
/// <remarks>
/// A interface abstrai a lógica de recuperação dos dados do usuário atualmente autenticado,
/// permitindo que diferentes implementações forneçam essa informação conforme o contexto
/// (por exemplo, via token JWT, sessão ou outro mecanismo de autenticação).
/// </remarks>
public interface IUsuarioAutenticado
{
    /// <summary>
    /// Obtém de forma assíncrona os dados do usuário autenticado no sistema.
    /// </summary>
    /// <returns>
    /// Uma tarefa que resulta em um objeto <see cref="UsuarioAutenticado"/> contendo
    /// o identificador, e-mail e a role do usuário autenticado.
    /// </returns>
    Task<UsuarioAutenticado> ObterUsuarioAutenticadoAsync(CancellationToken token);
}
