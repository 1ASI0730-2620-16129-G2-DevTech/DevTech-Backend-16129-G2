using DevTech.OrderManagement.Application.Internal.CommandServices;
using DevTech.OrderManagement.Application.Internal.QueryServices;
using DevTech.OrderManagement.Application.Services;
using DevTech.OrderManagement.Domain.Repositories;
using DevTech.OrderManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using DevTech.OrderManagement.Interfaces.REST.Transform;

namespace DevTech.OrderManagement.Infrastructure.Configuration;

/// <summary>
///     Dependency injection registrations of the Order Management bounded context.
/// </summary>
public static class OrderManagementServiceCollectionExtensions
{
    /// <summary>
    ///     Registers repositories, application services and REST assemblers of Order Management.
    ///     Event handlers (e.g. <c>PaymentConfirmedEventHandler</c>) are discovered by the mediator assembly scan.
    /// </summary>
    public static IServiceCollection AddOrderManagement(this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderCommandService, OrderCommandService>();
        services.AddScoped<IOrderQueryService, OrderQueryService>();
        services.AddScoped<ActionResultFromOrderResultAssembler>();
        return services;
    }
}
