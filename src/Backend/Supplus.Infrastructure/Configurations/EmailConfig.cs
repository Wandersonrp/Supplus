namespace Supplus.Infrastructure.Configurations;

/// <summary>
/// Configurações necessárias para a conexão e autenticação com o servidor SMTP.
/// Nota: 'Usuario' e 'Senha' devem ser configurados via User Secrets ou Environment Variables.
/// </summary>
public class EmailConfig
{
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string SenderEmail { get; set; }
    public bool EnableSsl { get; set; }
    public string Usuario { get; set; }
    public string Senha { get; set; }
}
