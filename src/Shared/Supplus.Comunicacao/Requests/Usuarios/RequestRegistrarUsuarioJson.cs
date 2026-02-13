using Supplus.Comunicacao.Enums;

namespace Supplus.Comunicacao.Requests.Usuarios;

public record RequestRegistrarUsuarioJson(string PrimeiroNome, string Sobrenome, string Email, Role Role);
