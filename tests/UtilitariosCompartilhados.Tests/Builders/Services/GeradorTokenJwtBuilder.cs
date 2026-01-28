using Moq;
using Supplus.Domain.Enums;
using Supplus.Domain.Services;

namespace UtilitariosCompartilhados.Tests.Builders.Services;

public class GeradorTokenJwtBuilder
{
    private readonly Mock<IGeradorAccessToken> _mock;

    public GeradorTokenJwtBuilder()
    {
        _mock = new Mock<IGeradorAccessToken>();
    }

    public void Gerar()
    {
        _mock
            .Setup(s => s.Gerar(It.IsAny<Guid>(), It.IsAny<Role>()))
            .Returns(Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
    }

    public IGeradorAccessToken Build() => _mock.Object;
}
