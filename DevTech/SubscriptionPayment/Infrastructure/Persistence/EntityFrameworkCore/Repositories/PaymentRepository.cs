using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Represents the payment repository in the Subscription &amp; Payment bounded context.
/// </summary>
/// <param name="context">The <see cref="AppDbContext" /> to use.</param>
public class PaymentRepository(AppDbContext context) : BaseRepository<Payment>(context), IPaymentRepository
{
    // <inheritdoc />
    public new async Task<Payment?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentId = new PaymentId(id);
        return await Context.Set<Payment>()
            .FirstOrDefaultAsync(payment => payment.Id == paymentId, cancellationToken);
    }

    // <inheritdoc />
    public async Task<IEnumerable<Payment>> ListByCustomerAsync(int? customerId,
        CancellationToken cancellationToken = default)
    {
        var payments = Context.Set<Payment>().AsNoTracking();
        if (customerId is not null)
            payments = payments.Where(payment => payment.CustomerId == customerId);
        return await payments.OrderByDescending(payment => payment.Id).ToListAsync(cancellationToken);
    }

    // <inheritdoc />
    public new async Task<Payment?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
    {
        var paymentId = new PaymentId(id);
        return await Context.Set<Payment>()
            .AsNoTracking()
            .FirstOrDefaultAsync(payment => payment.Id == paymentId, cancellationToken);
    }
}
