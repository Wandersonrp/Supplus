using Supplus.Comunicacao.Enums;

namespace Supplus.Comunicacao.Requests.Chamados;

public record RequestCriarChamadoJson(
    string Titulo, 
    string Descricao, 
    Prioridade Prioridade,    
    Guid IdCategoria);