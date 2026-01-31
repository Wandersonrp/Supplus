using Supplus.Domain.Entities;

namespace Supplus.Domain.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{    
    Task<Usuario?> ObterPorEmailAsync(string email);
}
