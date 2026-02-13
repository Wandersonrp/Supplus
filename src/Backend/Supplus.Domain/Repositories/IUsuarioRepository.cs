using Supplus.Domain.Entities;

namespace Supplus.Domain.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{    
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<bool> ExisteUsuarioComIdentificadorExternoAsync(Guid identificador);
    Task<bool> ExisteUsuarioComEmailAsync(string email, CancellationToken token);
}
