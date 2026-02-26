using Microsoft.AspNetCore.Authorization;
using Supplus.Api.Requirements;
using Supplus.Domain.Enums;

namespace Supplus.Api.Handlers;

/// <summary>
/// Handler de autorização responsável por validar se o usuário autenticado possui as permissões 
/// necessárias para acessar funcionalidades e realizar ações destinadas a agentes internos, como Administradores e Agentes de Suporte.
/// </summary>
/// <remarks>
/// Este handler verifica se o usuário possui os perfis de <see cref="Role.Administrador"/> 
/// ou <see cref="Role.AgenteSuporte"/> contidos nas Claims do JWT.
/// </remarks>
public class AgenteInternoHandler : AuthorizationHandler<AgenteInternoRequirement>
{
    /// <summary>
    /// Avalia o requisito de autorização verificando as Roles do usuário no contexto atual.
    /// </summary>
    /// <param name="context">O contexto de autorização que contém o <see cref="ClaimsPrincipal"/> (usuário logado).</param>
    /// <param name="requirement">O requisito específico que deve ser validado (neste caso, o registro de usuário).</param>
    /// <returns>Uma <see cref="Task"/> que representa o processo de avaliação assíncrona.</returns>
    /// <remarks>
    /// Se o usuário for um Administrador ou Agente de Suporte, o requisito é marcado como bem-sucedido. 
    /// A lógica detalhada de hierarquia (quem o suporte pode criar) deve ser tratada na camada de aplicação (Caso de Uso).
    /// </remarks>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgenteInternoRequirement requirement)
    {
        var usuario = context.User;

        if (usuario.IsInRole(nameof(Role.Administrador)) ||
            usuario.IsInRole(nameof(Role.AgenteSuporte)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
