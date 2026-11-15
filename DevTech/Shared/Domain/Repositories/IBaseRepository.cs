namespace DevTech.Shared.Domain.Repositories;

/// <summary>
///     Base repository interface for all repositories, with a configurable key type.
/// </summary>
/// <remarks>
///     Use this interface when the aggregate identifier is not an <see cref="int" />
///     (for example, a strongly-typed Guid identifier).
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
/// <typeparam name="TKey">
///     The type of the entity identifier. It must match the CLR type of the entity <c>Id</c> property
/// </typeparam>
public interface IBaseRepository<TEntity, in TKey>
{
    /// <summary>
    ///     Add an entity to the repository
    /// </summary>
    /// <param name="entity">
    ///     The entity to add
    /// </param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an entity by its id
    /// </summary>
    /// <param name="id">
    ///     The id of the entity to find
    /// </param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>
    ///     The entity if found, otherwise null
    /// </returns>
    Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an entity by its id without change tracking
    /// </summary>
    /// <param name="id">
    ///     The id of the entity to find
    /// </param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>
    ///     The entity if found, otherwise null
    /// </returns>
    Task<TEntity?> FindByIdAsNoTrackingAsync(TKey id, CancellationToken cancellationToken = default);

    void Update(TEntity entity);

    /// <summary>
    ///     Remove an entity from the repository
    /// </summary>
    /// <param name="entity">
    ///     The entity to remove
    /// </param>
    void Remove(TEntity entity);

    /// <summary>
    ///     List all entities in the repository
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>
    ///     A list of all entities in the repository
    /// </returns>
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     List all entities in the repository without change tracking
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>
    ///     A list of all entities in the repository
    /// </returns>
    Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default);
}

/// <summary>
///     Base repository interface for all repositories whose identifier is an <see cref="int" />
/// </summary>
/// <remarks>
///     This interface is used to define the basic CRUD operations for all repositories.
///     It keeps working as before for the existing repositories.
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int>
{
}
