using Supplus.Domain.Enums;

namespace Supplus.Domain.Entities;

public class EntidadeBase
{
    public long Id { get; private set; }
    public Guid IdExterno { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public long? CriadoPor { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }
    public long? AtualizadoPor { get; private set; }
    public DateTime? DeletadoEm { get; private set; }
    public long? DeletadoPor { get; private set; }

    public string? MotivoDelecao { get; private set; }
    public Status Status { get; private set; }

    public EntidadeBase(long? criadoPor = null)
    {
        IdExterno = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        Status = Status.Ativo;
        CriadoPor = criadoPor;
    }

    protected virtual void DefinirAtualizadoEm(long atualizadoPor)
    {
        AtualizadoEm = DateTime.UtcNow;
        AtualizadoPor = atualizadoPor;
    }

    protected virtual void DefinirAtualizadoEm () => AtualizadoEm = DateTime.UtcNow;

    protected virtual void Inativar(long? deletadoPor = null, string? motivoDelecao = null)
    {
        DeletadoEm = DateTime.UtcNow;
        DeletadoPor = deletadoPor;
        MotivoDelecao = motivoDelecao;
        Status = Status.Inativo;
    }
}
