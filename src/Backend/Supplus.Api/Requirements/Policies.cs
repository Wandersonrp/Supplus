namespace Supplus.Api.Requirements;

/// <summary>
/// Centraliza as definições de nomes de políticas de autorização utilizadas em toda a aplicação.
/// </summary>
/// <remarks>
/// Estas constantes devem ser utilizadas nos atributos <see cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute"/> 
/// dos Controllers e na configuração de segurança no Program.cs.
/// </remarks>
public class Policies
{
    /// <summary>
    /// Política que valida se o usuário autenticado possui permissão para registrar novos usuários, 
    /// respeitando a hierarquia de cargos (ex: Admin ou Suporte).
    /// </summary>
    public const string PodeRegistrarUsuario = "PodeRegistrarUsuario";
}
