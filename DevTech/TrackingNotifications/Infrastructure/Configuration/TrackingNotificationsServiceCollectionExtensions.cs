using DevTech.TrackingNotifications.Application.CommandServices;
using DevTech.TrackingNotifications.Application.Internal.CommandServices;
using DevTech.TrackingNotifications.Application.Internal.QueryServices;
using DevTech.TrackingNotifications.Application.QueryServices;
using DevTech.TrackingNotifications.Domain.Repositories;
using DevTech.TrackingNotifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace DevTech.TrackingNotifications.Infrastructure.Configuration;

/// <summary>
///     Dependency injection registrations of the Tracking &amp; Notifications bounded context.
/// </summary>
public static class TrackingNotificationsServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the repository and the application services of Tracking &amp; Notifications.
    ///     The event handlers (<c>StageAdvancedEventHandler</c>, <c>LaundryOrderReceivedEventHandler</c>)
    ///     are discovered by the mediator assembly scan.
    /// </summary>
    public static IServiceCollection AddTrackingNotifications(this IServiceCollection services)
    {
        services.AddScoped<IOrderTrackingRepository, OrderTrackingRepository>();
        services.AddScoped<IOrderTrackingCommandService, OrderTrackingCommandService>();
        services.AddScoped<IOrderTrackingQueryService, OrderTrackingQueryService>();
        return services;
    }
}
