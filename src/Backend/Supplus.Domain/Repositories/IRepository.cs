using Supplus.Domain.Entities;

namespace Supplus.Domain.Repositories;

/// <summary>
/// Define um contrato genérico para repositórios de entidades que herdam de <see cref="EntidadeBase"/>.
/// </summary>
/// <typeparam name="TEntidade">
/// Tipo da entidade que o repositório manipula. Deve herdar de <see cref="EntidadeBase"/>.
/// </typeparam>
public interface IRepository<TEntidade> where TEntidade : EntidadeBase
{
    /// <summary>
    /// Obtém uma entidade pelo identificador externo (GUID).
    /// Retorna <c>null</c> caso não seja encontrada.
    /// </summary>
    /// <param name="idExterno">Identificador externo único da entidade.</param>
    /// <returns>Instância da entidade ou <c>null</c> se não existir.</returns>
    Task<TEntidade?> ObterPorIdExternoAsync(Guid idExterno);

    /// <summary>
    /// Obtém uma entidade pelo identificador interno (chave primária).
    /// Retorna <c>null</c> caso não seja encontrada.
    /// </summary>
    /// <param name="id">Identificador interno da entidade.</param>
    /// <returns>Instância da entidade ou <c>null</c> se não existir.</returns>
    Task<TEntidade?> ObterPorIdAsync(long id);
}

