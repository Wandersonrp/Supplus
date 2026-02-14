using Moq;
using Supplus.Application.Services.Notificacoes;

namespace UtilitariosCompartilhados.Tests.Builders.Services;

public class EmailServiceBuilder
{
    private readonly Mock<IEmailService> _mock;

    public EmailServiceBuilder()
    {
        _mock = new Mock<IEmailService>();
    }

    public IEmailService Build() => _mock.Object;
}
