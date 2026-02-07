using Supplus.Domain.Enums;

namespace Supplus.Domain.Models;

/// <summary>
/// Representa um usuário autenticado no sistema.
/// </summary>
/// <remarks>
/// Contém informações básicas de identificação, incluindo o identificador interno
/// e o endereço de e-mail associado. Este modelo é utilizado para representar
/// o usuário após a autenticação bem-sucedida.
/// </remarks>
/// <param name="Id">Identificador único do usuário no domínio.</param>
/// <param name="Email">Endereço de e-mail do usuário autenticado.</param>
/// <param name="Role">Role do usuário autenticado.</param>
public record UsuarioAutenticado(long Id, string Email, Role Role);