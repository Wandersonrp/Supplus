namespace Supplus.Domain.Services.Tokens;

/// <summary>
/// Define o contrato para validação de tokens de acesso (JWT).
/// </summary>
/// <remarks>
/// Responsável por validar o token fornecido e extrair o identificador único do usuário
/// associado. Implementações desta interface devem garantir a integridade e autenticidade
/// do token antes de retornar o identificador.
/// </remarks>
/// <param name="token">Token JWT a ser validado.</param>
/// <returns>
/// O identificador único do usuário (<see cref="Guid"/>) extraído do token, caso seja válido.
/// </returns>
public interface IValidadorAccessToken
{
    /// <summary>
    /// Valida o token de acesso fornecido e retorna o identificador único do usuário associado.
    /// </summary>
    /// <param name="token">Token JWT a ser validado.</param>
    /// <returns>
    /// O identificador do usuário (<see cref="Guid"/>) extraído do token, caso seja válido.
    /// </returns>
    Guid ValidarEObterIdUsuario(string token);
}

