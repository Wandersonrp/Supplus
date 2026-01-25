using Supplus.Domain.Entities;

namespace Supplus.Domain.Repositories;

public interface IUsuarioRepository
{    
    Task<Usuario?> ObterPorEmailAsync(string email);
}
