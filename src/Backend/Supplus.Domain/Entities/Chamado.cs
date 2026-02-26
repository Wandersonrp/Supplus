using Supplus.Domain.Enums;
using Supplus.Domain.Exceptions;

namespace Supplus.Domain.Entities;

public sealed class Chamado : EntidadeBase
{
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public StatusChamado StatusChamado { get; private set; }
    public Prioridade Prioridade { get; private set; }
    public DateTime AbertoEm { get; private set; }
    public DateTime? ResolvidoEm { get; private set; }
    public DateTime? FechadoEm { get; private set; }

    public long IdUsuarioComum { get; private set; }
    public Usuario UsuarioComum { get; private set; }

    public long IdAgente { get; private set; }
    public Usuario Agente { get; private set; }

    public long IdCategoria { get; private set; }
    public Categoria Categoria { get; private set; }

    private Chamado()
    {
        StatusChamado = StatusChamado.Aberto;
        AbertoEm = DateTime.UtcNow;
    }

    public Chamado(string titulo, string descricao, Prioridade prioridade, long idUsuarioComum, long idCategoria)
    {
        DomainException.When(string.IsNullOrWhiteSpace(titulo), "O título do chamado é obrigatório.");
        DomainException.When(string.IsNullOrWhiteSpace(descricao), "A descrição do chamado é obrigatória.");

        Titulo = titulo;
        Descricao = descricao;
        Prioridade = prioridade;
        IdUsuarioComum = idUsuarioComum;
        IdCategoria = idCategoria;
    }

    public void AtribuirAgente(long idAgente)
    {
        DomainException.When(StatusChamado != StatusChamado.Aberto, "Somente chamados abertos podem ser atribuídos a um agente.");
        
        IdAgente = idAgente;
        StatusChamado = StatusChamado.EmAndamento;
        DefinirAtualizadoEm(idAgente);
    }
}
