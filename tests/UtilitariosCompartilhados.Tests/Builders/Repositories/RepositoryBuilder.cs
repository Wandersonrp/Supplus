using Moq;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;

namespace UtilitariosCompartilhados.Tests.Builders.Repositories;

public class RepositoryBuilder<TEntidade> where TEntidade : EntidadeBase
{
    private readonly Mock<IRepository<TEntidade>> _mock;

    public RepositoryBuilder()
    {
        _mock = new Mock<IRepository<TEntidade>>();
    }

    public void ObterPorIdAsync(TEntidade entidade)
    {
        _mock.Setup(r => r.ObterPorIdAsync(entidade.Id)).ReturnsAsync(entidade);
    }

    public IRepository<TEntidade> Build() => _mock.Object;
}
