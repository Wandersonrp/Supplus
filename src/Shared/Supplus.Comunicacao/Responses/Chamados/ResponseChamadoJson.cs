using Supplus.Comunicacao.Enums;

namespace Supplus.Comunicacao.Responses.Chamados;

public record ResponseChamadoJson(
    Guid Id, 
    string Titulo, 
    string Descricao, 
    StatusChamado StatusChamado, 
    Prioridade Prioridade, 
    DateTime AbertoEm, 
    DateTime? ResolvidoEm = null, 
    DateTime? FechadoEm = null);
