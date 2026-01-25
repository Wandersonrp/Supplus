using Moq;
using Supplus.Domain.Repositories;

namespace UtilitariosCompartilhados.Tests.Builders.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();        
        return mock.Object;
    }
}
