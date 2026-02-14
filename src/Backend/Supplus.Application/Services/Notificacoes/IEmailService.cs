using Supplus.Application.DTOs;

namespace Supplus.Application.Services.Notificacoes;

public interface IEmailService
{
    Task EnviarAsync(EmailDTO email);
}
