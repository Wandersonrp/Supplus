using Moq;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;

namespace UtilitariosCompartilhados.Tests.Builders.Repositories;

public class RefreshTokenRepositoryBuilder
{
    private readonly Mock<IRefreshTokenRepository> _mock;

    public RefreshTokenRepositoryBuilder()
    {
        _mock = new Mock<IRefreshTokenRepository>();
    }

    public void ObterRefreshTokenPorTokenAsync(RefreshToken refreshToken)
    {
        _mock
            .Setup(r => r.ObterRefreshTokenPorTokenAsync(refreshToken.Token, CancellationToken.None))
            .ReturnsAsync(refreshToken);
    }

    public IRefreshTokenRepository Build() => _mock.Object;
}