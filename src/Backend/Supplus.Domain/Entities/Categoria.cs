namespace Supplus.Domain.Entities;

public sealed class Categoria : EntidadeBase
{    
    public string Nome { get; private set; }
    public string Descricao { get; private set; }

    private readonly List<Chamado> _chamados = new();
    public IReadOnlyCollection<Chamado>  Chamados => _chamados.AsReadOnly();

    private Categoria() { }

    public Categoria(long criadoPor, string nome, string descricao) : base(criadoPor)
    {        
        Nome = nome;
        Descricao = descricao;
    }
}
