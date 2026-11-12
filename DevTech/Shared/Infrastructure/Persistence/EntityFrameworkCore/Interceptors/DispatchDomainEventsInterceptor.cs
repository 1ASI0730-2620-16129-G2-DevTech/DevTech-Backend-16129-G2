using Cortex.Mediator;
using Cortex.Mediator.Notifications;
using DevTech.Shared.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

// In-process integration only. Use an outbox if durable cross-service delivery is needed.
public sealed class DispatchDomainEventsInterceptor(IMediator mediator,
    ILogger<DispatchDomainEventsInterceptor> logger) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
        int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return result;
        var roots = eventData.Context.ChangeTracker.Entries<IAggregateRoot>().Select(x => x.Entity).ToList();
        var events = roots.SelectMany(x => x.DomainEvents).Cast<INotification>().ToList();
        foreach (var root in roots) root.ClearDomainEvents();
        foreach (var notification in events)
        {
            try { await mediator.PublishAsync(notification, CancellationToken.None); }
            catch (Exception exception)
            {
                // Database commit already succeeded: never tell the caller to retry the mutation.
                logger.LogError(exception, "Post-commit event {EventType} failed. No automatic retry configured.", notification.GetType().Name);
            }
        }
        return result;
    }
}

