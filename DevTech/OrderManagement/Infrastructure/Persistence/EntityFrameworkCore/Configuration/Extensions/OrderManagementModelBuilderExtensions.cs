using DevTech.OrderManagement.Domain.Model.Aggregates;
using DevTech.OrderManagement.Domain.Model.Entities;
using DevTech.OrderManagement.Domain.Model.ValueObjects;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace DevTech.OrderManagement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core mapping of the Order Management bounded context.
/// </summary>
/// <remarks>
///     Tables (<c>orders</c>, <c>garment_items</c>) and column names are converted to snake_case afterwards by
///     <c>UseSnakeCaseNamingConvention</c>, which <c>AppDbContext</c> applies after every context configuration.
///     Index names are written already in snake_case on purpose, so that convention leaves them untouched.
/// </remarks>
public static class OrderManagementModelBuilderExtensions
{
    public static void ApplyOrderManagementConfiguration(this ModelBuilder builder)
    {
        var orderIdConverter = new StronglyTypedIdConverter<OrderId, Guid>(value => new OrderId(value));
        var garmentItemIdConverter =
            new StronglyTypedIdConverter<GarmentItemId, Guid>(value => new GarmentItemId(value));

        builder.Entity<Order>(order =>
        {
            order.HasKey(o => o.Id);
            order.Property(o => o.Id).HasConversion(orderIdConverter).ValueGeneratedNever();

            order.Property(o => o.CustomerId).IsRequired();
            order.Property(o => o.LaundryId).IsRequired();
            order.Property(o => o.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            order.Property(o => o.DeliveryMethod).HasConversion<string>().HasMaxLength(20).IsRequired();
            order.Property(o => o.ServiceType).HasConversion<string>().HasMaxLength(30).IsRequired();
            order.Property(o => o.EstimatedDeliveryDate).IsRequired();
            order.Property(o => o.SpecialCareInstructions).HasMaxLength(Order.MaxSpecialCareInstructionsLength);

            // Domain events live only in memory; they are dispatched by DispatchDomainEventsInterceptor.
            order.Ignore(o => o.DomainEvents);

            order.HasIndex(o => o.CustomerId).HasDatabaseName("ix_orders_customer_id");
            order.HasIndex(o => o.LaundryId).HasDatabaseName("ix_orders_laundry_id");
            order.HasIndex(o => o.Status).HasDatabaseName("ix_orders_status");

            order.HasMany(o => o.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // The aggregate exposes Items as read-only; EF fills the private _items list directly.
            order.Navigation(o => o.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<GarmentItem>(item =>
        {
            item.HasKey(i => i.Id);
            item.Property(i => i.Id).HasConversion(garmentItemIdConverter).ValueGeneratedNever();
            item.Property(i => i.OrderId).HasConversion(orderIdConverter).IsRequired();

            item.Property(i => i.Type).HasMaxLength(GarmentItem.MaxTypeLength).IsRequired();
            item.Property(i => i.Quantity).IsRequired();
            item.Property(i => i.CareInstructions).HasMaxLength(GarmentItem.MaxCareInstructionsLength);
        });
    }
}
