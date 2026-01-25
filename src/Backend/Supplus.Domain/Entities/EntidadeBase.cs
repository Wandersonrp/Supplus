using Supplus.Domain.Enums;

namespace Supplus.Domain.Entities;

public class EntidadeBase
{
    public long Id { get; private set; }
    public Guid IdExterno { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    public Status Status { get; private set; }

    public EntidadeBase()
    {
        IdExterno = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        Status = Status.Ativo;
    }

    protected virtual void DefinirAtualizadoEm() => AtualizadoEm = DateTime.UtcNow;
}
