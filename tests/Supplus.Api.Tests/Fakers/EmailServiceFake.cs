using Supplus.Application.DTOs;
using Supplus.Application.Services.Notificacoes;

namespace Supplus.Api.Tests.Fakers;

public class EmailServiceFake : IEmailService
{
    public Task EnviarAsync(EmailDTO email) => Task.CompletedTask;
}
