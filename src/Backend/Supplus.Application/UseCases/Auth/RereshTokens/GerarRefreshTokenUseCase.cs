using Supplus.Comunicacao.Requests.RefreshTokens;
using Supplus.Comunicacao.Responses.Auth;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;
using Supplus.Domain.Services.Tokens;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Auth.RereshTokens;

public sealed class GerarRefreshTokenUseCase : IGerarRefreshTokenUseCase
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Usuario> _usuarioRepository;
    private readonly IGeradorAccessToken _geradorAccessToken;

    public GerarRefreshTokenUseCase(
        IRefreshTokenRepository refreshTokenRepository, 
        IUnitOfWork unitOfWork,
        IRepository<Usuario> usuarioRepository, 
        IGeradorAccessToken geradorAccessToken)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _usuarioRepository = usuarioRepository;
        _geradorAccessToken = geradorAccessToken;
    }

    /// <summary>
    /// Processa a troca de um refresh token por um novo par de tokens (access token + refresh token).
    /// </summary>
    /// <remarks>
    /// Verifica se a operação foi cancelada via <paramref name="token"/>. Busca o refresh token fornecido,
    /// valida se ele existe e está ativo; se detectar tentativa de reuso (token revogado ou já usado),
    /// aplica medidas de segurança adicionais (revoga todos os refresh tokens do usuário) e retorna falha.
    /// Se o token for válido, marca o token antigo como usado, gera um novo access token e um novo refresh token,
    /// persiste as alterações e retorna os tokens gerados. As alterações são confirmadas via Unit of Work.
    /// </remarks>
    /// <param name="request">Dados da requisição contendo o refresh token recebido e metadados do dispositivo.</param>
    /// <param name="token">Token de cancelamento para abortar a operação assincronamente.</param>
    /// <returns>
    /// Um <see cref="ResultadoPersonalizado{ResponseTokenJson}"/> contendo os novos tokens em caso de sucesso;
    /// caso contrário, um resultado de falha com o erro padronizado apropriado (por exemplo, não autorizado).
    /// </returns>
    public async Task<ResultadoPersonalizado<ResponseTokenJson>> Executar(RequestNovoTokenJson request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var refreshTokenExistente = await _refreshTokenRepository
            .ObterRefreshTokenPorTokenAsync(request.RefreshToken, token);

        // Validar se o refresh token existe e se está ativo
        if (refreshTokenExistente is null || !refreshTokenExistente.EstaAtivo)
        {
            if (refreshTokenExistente is not null &&
                (refreshTokenExistente.FoiRevogado || refreshTokenExistente.FoiUsado))
            {
                // Implementa lógica de segurança adicional, como revogar todos os tokens associados ao usuário
                await _refreshTokenRepository
                    .RevogarTodosRefreshTokensDoUsuarioAsync(refreshTokenExistente.IdUsuario);

                await _unitOfWork.CommitAsync();

                return ResultadoPersonalizado<ResponseTokenJson>
                    .Falha(ErroPadronizado.NaoAutorizadoErro("Tentativa de reuso de sessão detectada."));
            }

            return ResultadoPersonalizado<ResponseTokenJson>
                    .Falha(ErroPadronizado.NaoAutorizadoErro("Refresh Token inválido ou expirado."));
        }

        refreshTokenExistente.MarcarComoUsado();

        var usuario = await _usuarioRepository.ObterPorIdAsync(refreshTokenExistente.IdUsuario);

        if (usuario is null)
            return ResultadoPersonalizado<ResponseTokenJson>.Falha(ErroPadronizado.NaoAutorizadoErro());

        // Gera novos tokens (Access Token e Refresh Token)
        var accessToken = _geradorAccessToken.Gerar(usuario.IdExterno, usuario.Role);

        var novoRefreshToken = new RefreshToken(usuario.Id, request.DispositivoInfo, request.Ip);

        await _refreshTokenRepository.AdicionarAsync(novoRefreshToken);

        await _unitOfWork.CommitAsync();

        var tokens = new ResponseTokenJson(accessToken, novoRefreshToken.Token);

        return ResultadoPersonalizado<ResponseTokenJson>.Sucesso(tokens);
    }
}
