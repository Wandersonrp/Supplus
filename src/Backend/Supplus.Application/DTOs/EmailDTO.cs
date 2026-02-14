namespace Supplus.Application.DTOs;

public record EmailDTO(string Destinatario, string Assunto, string NomeTemplate, Dictionary<string, string> Dados);