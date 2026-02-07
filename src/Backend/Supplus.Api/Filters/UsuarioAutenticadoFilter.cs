using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Supplus.Comunicacao.Responses;
using Supplus.Domain.Repositories;
using Supplus.Domain.Services.Tokens;
using Supplus.Exceptions.Exceptions;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Api.Filters;

public class UsuarioAutenticadoFilter : IAsyncAuthorizationFilter
{
    private readonly IValidadorAccessToken _validadorAccessToken;
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioAutenticadoFilter(
        IValidadorAccessToken validadorAccessToken, 
        IUsuarioRepository usuarioRepository)
    {
        _validadorAccessToken = validadorAccessToken;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Executa a lógica de autorização baseada em token JWT para a requisição atual.
    /// </summary>
    /// <remarks>
    /// Obtém o token do cabeçalho Authorization, valida-o e extrai o identificador do usuário.
    /// Em seguida, verifica se o usuário correspondente existe no repositório. Caso não exista,
    /// ou se ocorrerem erros de validação, define o resultado da requisição como não autorizado,
    /// retornando uma resposta JSON com a mensagem de erro apropriada. Também trata cenários de
    /// token expirado e exceções específicas da aplicação.
    /// </remarks>
    /// <param name="context">Contexto do filtro de autorização que contém informações da requisição.</param>
    /// <returns>
    /// Uma tarefa assíncrona que representa a execução da autorização. Em caso de falha, o
    /// <see cref="AuthorizationFilterContext.Result"/> é definido com uma resposta de erro.
    /// </returns>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = ObterTokenRequest(context);

            var identificadorUsuario = _validadorAccessToken.ValidarEObterIdUsuario(token);

            var existeUsuario = await _usuarioRepository
                .ExisteUsuarioComIdentificadorExternoAsync(identificadorUsuario);

            if (!existeUsuario)
                throw new SupplusException(MensagensErro.PERMISSOES_INVALIDAS);
        }
        catch (SupplusException ex)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson(ex.Message));
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson("Token expirado."));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErroJson(MensagensErro.PERMISSOES_INVALIDAS));
        }
    }


    /// <summary>
    /// Obtém o token JWT da requisição HTTP a partir do cabeçalho Authorization.
    /// </summary>
    /// <remarks>
    /// Verifica se o cabeçalho Authorization está presente e não vazio. Caso contrário,
    /// lança uma exceção indicando token inválido. Quando válido, remove o prefixo "Bearer "
    /// e retorna apenas o valor do token limpo.
    /// </remarks>
    /// <param name="context">Contexto do filtro de autorização contendo a requisição atual.</param>
    /// <returns>O valor do token JWT extraído do cabeçalho Authorization.</returns>
    /// <exception cref="SupplusException">
    /// Lançada quando o cabeçalho Authorization está ausente ou vazio.
    /// </exception>
    private static string ObterTokenRequest(AuthorizationFilterContext context)
    {
        var auhtorization = context.HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(auhtorization))
            throw new SupplusException(MensagensErro.TOKEN_INVALIDO);

        return auhtorization["Bearer ".Length..].Trim();
    }

}
