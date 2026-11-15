using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using DevTech.PickupsDeliveries.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.PickupsDeliveries.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     EF Core implementation of the PickupDelivery repository.
/// </summary>
/// <remarks>
///     The basic operations (add, find by id, update, remove, list) are inherited from BaseRepository.
///     This class only adds the searches specific to this bounded context.
/// </remarks>
/// <param name="context">The application database context</param>
public class PickupDeliveryRepository(AppDbContext context)
    : BaseRepository<PickupDelivery>(context), IPickupDeliveryRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<PickupDelivery>> FindByOrderIdAsync(Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<PickupDelivery>()
            .Where(pickupDelivery => pickupDelivery.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PickupDelivery?> FindByOrderIdAndTypeAsync(Guid orderId, ServiceType type,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<PickupDelivery>()
            .FirstOrDefaultAsync(
                pickupDelivery =>
                    pickupDelivery.OrderId == orderId
                    && pickupDelivery.Type == type,
                cancellationToken
            );
    }
}
