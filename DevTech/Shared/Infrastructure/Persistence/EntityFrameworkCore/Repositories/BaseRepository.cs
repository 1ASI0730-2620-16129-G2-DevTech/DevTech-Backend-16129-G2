using System.Linq.Expressions;
using DevTech.Shared.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Base repository for all repositories, with a configurable key type.
/// </summary>
/// <remarks>
///     This class is used to define the basic CRUD operations for all repositories.
///     This class implements the IBaseRepository interface.
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
/// <typeparam name="TKey">
///     The type of the entity identifier. It must match the CLR type of the entity <c>Id</c> property
/// </typeparam>
public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
{
    protected readonly AppDbContext Context;

    /// <summary>
    ///     Default constructor for the base repository
    /// </summary>
    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    // inheritedDoc
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    // inheritedDoc
    public async Task<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(new object[] { id! }, cancellationToken);
    }

    // inheritedDoc
    public async Task<TEntity?> FindByIdAsNoTrackingAsync(TKey id, CancellationToken cancellationToken = default)
    {
        // Equivalent to: e => EF.Property<TKey>(e, "Id") == id
        var entity = Expression.Parameter(typeof(TEntity), "e");
        var idProperty = Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(TKey) }, entity,
            Expression.Constant("Id"));
        var body = Expression.Equal(idProperty, Expression.Constant(id, typeof(TKey)));
        var predicate = Expression.Lambda<Func<TEntity, bool>>(body, entity);

        return await Context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    // inheritedDoc
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    // inheritedDoc
    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    // inheritedDoc
    public async Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    // inheritedDoc
    public async Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

/// <summary>
///     Base repository for all repositories whose identifier is an <see cref="int" />
/// </summary>
/// <remarks>
///     It keeps working as before for the existing repositories.
/// </remarks>
/// <typeparam name="TEntity">
///     The entity type for the repository
/// </typeparam>
public class BaseRepository<TEntity> : BaseRepository<TEntity, int>, IBaseRepository<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     Default constructor for the base repository
    /// </summary>
    protected BaseRepository(AppDbContext context) : base(context)
    {
    }
}
