using Moq;
using Supplus.Domain.Models;
using Supplus.Domain.Services;

namespace UtilitariosCompartilhados.Tests.Builders.Services;

public class UsuarioAutenticadoBuilder
{
    private readonly Mock<IUsuarioAutenticado> _mock;

    public UsuarioAutenticadoBuilder()
    {
        _mock = new Mock<IUsuarioAutenticado>();
    }

    public void ObterUsuarioAutenticadoAsync(UsuarioAutenticado usuarioAutenticado)
    {
        _mock.Setup(s => s.ObterUsuarioAutenticadoAsync()).ReturnsAsync(usuarioAutenticado);
    }

    public IUsuarioAutenticado Build() => _mock.Object;
}
