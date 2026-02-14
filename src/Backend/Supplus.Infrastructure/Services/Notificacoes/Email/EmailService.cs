using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supplus.Application.DTOs;
using Supplus.Application.Services.Notificacoes;
using Supplus.Infrastructure.Configurations;
using System.Net;
using System.Net.Mail;

namespace Supplus.Infrastructure.Services.Notificacoes.Email;

public class EmailService : IEmailService
{
    private readonly EmailConfig _emailConfig;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailConfig> emailConfig, ILogger<EmailService> logger)
    {
        _emailConfig = emailConfig.Value;
        _logger = logger;
    }

    public async Task EnviarAsync(EmailDTO email)
    {
        var corpoEmail = CarregarTemplate(email.NomeTemplate, email.Dados);

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailConfig.SenderEmail),
            Subject = email.Assunto,
            Body = corpoEmail,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email.Destinatario);

        using var smtpClient = ConfigSmtpClient();

        try
        {
           await smtpClient.SendMailAsync(mailMessage);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {Destinatario} com assunto {Assunto}", email.Destinatario, email.Assunto);
            throw;
        }
    }

    private SmtpClient ConfigSmtpClient()
    {
        return new SmtpClient(_emailConfig.SmtpServer)
        {
            Port = _emailConfig.Port,
            Credentials = new NetworkCredential(_emailConfig.Usuario, _emailConfig.Senha),
            EnableSsl = _emailConfig.EnableSsl,
        };
    }

    private string CarregarTemplate(string nomeTemplate, Dictionary<string, string> placeholders)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "Notificacoes", "Email", "Templates", $"{nomeTemplate}.html");
        
        if(!File.Exists(path))
            throw new FileNotFoundException($"Template de email '{nomeTemplate}' não encontrado em '{path}'.");

        var html = File.ReadAllText(path);

        foreach(var item in placeholders)
            html = html.Replace($"{{{{{item.Key}}}}}", item.Value);

        return html;
    }
}
