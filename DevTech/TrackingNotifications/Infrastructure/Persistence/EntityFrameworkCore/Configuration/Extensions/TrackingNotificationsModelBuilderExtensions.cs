using DevTech.TrackingNotifications.Domain.Model.Aggregates;
using DevTech.TrackingNotifications.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTech.TrackingNotifications.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     EF Core mapping of the Tracking &amp; Notifications bounded context.
/// </summary>
/// <remarks>
///     Tables (<c>order_trackings</c>, <c>tracking_stage_records</c>) and columns are converted to snake_case by
///     <c>UseSnakeCaseNamingConvention</c>, which <c>AppDbContext</c> applies after every context configuration.
/// </remarks>
public static class TrackingNotificationsModelBuilderExtensions
{
    public static void ApplyTrackingNotificationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<OrderTracking>(tracking =>
        {
            tracking.HasKey(t => t.Id);
            tracking.Property(t => t.Id).ValueGeneratedOnAdd();
            tracking.Property(t => t.OrderId).IsRequired();
            tracking.Property(t => t.CurrentStage).HasConversion<string>().HasMaxLength(20).IsRequired();
            tracking.Property(t => t.EstimatedDelivery);

            // Domain events live only in memory; they are dispatched by DispatchDomainEventsInterceptor.
            tracking.Ignore(t => t.DomainEvents);

            // One tracking per order.
            tracking.HasIndex(t => t.OrderId).IsUnique().HasDatabaseName("ux_order_trackings_order_id");

            tracking.HasMany(t => t.History)
                .WithOne()
                .HasForeignKey(record => record.OrderTrackingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // The aggregate exposes History as read-only; EF fills the private _history list directly.
            tracking.Navigation(t => t.History).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<TrackingStageRecord>(record =>
        {
            record.HasKey(r => r.Id);
            record.Property(r => r.Id).ValueGeneratedOnAdd();
            record.Property(r => r.Stage).HasConversion<string>().HasMaxLength(20).IsRequired();
            record.Property(r => r.OccurredAt).IsRequired();
        });
    }
}
