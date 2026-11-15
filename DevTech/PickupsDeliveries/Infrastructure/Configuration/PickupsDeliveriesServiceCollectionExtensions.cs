using DevTech.PickupsDeliveries.Application.Internal.CommandServices;
using DevTech.PickupsDeliveries.Application.Internal.OutboundServices;
using DevTech.PickupsDeliveries.Application.Internal.QueryServices;
using DevTech.PickupsDeliveries.Application.Outbound;
using DevTech.PickupsDeliveries.Application.Services;
using DevTech.PickupsDeliveries.Domain.Repositories;
using DevTech.PickupsDeliveries.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace DevTech.PickupsDeliveries.Infrastructure.Configuration;

/// <summary>
///     Dependency injection registrations of the Pickups and Deliveries bounded context.
/// </summary>
public static class PickupsDeliveriesServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the repository, the application services and the anti-corruption layer
    ///     of Pickups and Deliveries.
    /// </summary>
    /// <remarks>
    ///     The anti-corruption layer needs the <c>IOrdersContextFacade</c> registered by <c>AddOrderManagement</c>.
    /// </remarks>
    /// <param name="services">The service collection of the application</param>
    /// <returns>The same service collection, to allow chaining calls</returns>
    public static IServiceCollection AddPickupsDeliveries(this IServiceCollection services)
    {
        services.AddScoped<IPickupDeliveryRepository, PickupDeliveryRepository>();
        services.AddScoped<IPickupDeliveryCommandService, PickupDeliveryCommandService>();
        services.AddScoped<IPickupDeliveryQueryService, PickupDeliveryQueryService>();
        services.AddScoped<IOrderStatusFacade, OrderStatusAntiCorruptionLayer>();
        return services;
    }
}
