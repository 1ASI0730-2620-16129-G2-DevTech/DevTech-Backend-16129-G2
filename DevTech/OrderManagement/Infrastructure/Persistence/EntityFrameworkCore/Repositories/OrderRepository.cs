using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.OrderManagement.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.OrderManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of <see cref="IOrderRepository" />.
/// </summary>
public class OrderRepository(AppDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
    /// <inheritdoc />
    public async Task<Order?> FindByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .Include(order => order.Items)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> FindByCustomerIdAsync(Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .AsNoTracking()
            .Include(order => order.Items)
            .Where(order => order.CustomerId == customerId)
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Order>> FindByLaundryIdAsync(Guid laundryId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Order>()
            .AsNoTracking()
            .Include(order => order.Items)
            .Where(order => order.LaundryId == laundryId)
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
