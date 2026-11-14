using DevTech.SubscriptionPayment.Application.CommandServices;
using DevTech.SubscriptionPayment.Application.Internal.CommandServices;
using DevTech.SubscriptionPayment.Application.Internal.OutboundServices;
using DevTech.SubscriptionPayment.Application.Internal.QueryServices;
using DevTech.SubscriptionPayment.Application.QueryServices;
using DevTech.SubscriptionPayment.Domain.Repositories;
using DevTech.SubscriptionPayment.Infrastructure.PaymentGateway;
using DevTech.SubscriptionPayment.Infrastructure.PaymentGateway.External;
using DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace DevTech.SubscriptionPayment.Infrastructure.Configuration;

/// <summary>
///     Dependency injection registrations of the Subscription &amp; Payment bounded context.
/// </summary>
public static class SubscriptionPaymentServiceCollectionExtensions
{
    /// <summary>
    ///     Registers repositories, application services and the payment gateway of Subscription &amp; Payment.
    ///     Event handlers (e.g. <c>PaymentConfirmedEventHandler</c>) are discovered by the mediator assembly scan.
    /// </summary>
    public static IServiceCollection AddSubscriptionPayment(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Command and query services
        services.AddScoped<ISubscriptionPlanCommandService, SubscriptionPlanCommandService>();
        services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();
        services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
        services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
        services.AddScoped<IPaymentCommandService, PaymentCommandService>();
        services.AddScoped<IPaymentQueryService, PaymentQueryService>();

        // Payment gateway: port (IPaymentGateway) -> adapter -> anti-corruption layer -> external provider
        services.AddScoped<IExternalPaymentProviderApi, SimulatedExternalPaymentProviderApi>();
        services.AddScoped<PaymentGatewayAcl>();
        services.AddScoped<IPaymentGateway, PaymentGatewayAdapter>();

        return services;
    }
}
