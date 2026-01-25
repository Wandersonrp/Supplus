using Moq;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;

namespace UtilitariosCompartilhados.Tests.Builders.Repositories;

public class UsuarioRepositoryBuilder
{
    private readonly Mock<IUsuarioRepository> _mock;

    public UsuarioRepositoryBuilder()
    {
        _mock = new Mock<IUsuarioRepository>();
    }

    public void ObterPorEmailAsync(Usuario usuario)
    {
        _mock.Setup(r => r.ObterPorEmailAsync(usuario.Email)).ReturnsAsync(usuario);
    }

    public IUsuarioRepository Build() => _mock.Object;
}
