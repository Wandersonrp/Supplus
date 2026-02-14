using Supplus.Comunicacao.Enums;

namespace Supplus.Comunicacao.Responses.Usuarios;

public record class ResponseUsuarioJson(
    Guid Id, 
    string PrimeiroNome, 
    string Sobrenome, 
    string Email, 
    Role Role, 
    string NomeCompleto);