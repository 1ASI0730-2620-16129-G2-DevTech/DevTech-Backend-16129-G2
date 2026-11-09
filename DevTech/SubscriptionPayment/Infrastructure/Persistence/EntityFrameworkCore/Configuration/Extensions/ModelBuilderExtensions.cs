using DevTech.SubscriptionPayment.Domain.Model.Aggregates;
using DevTech.SubscriptionPayment.Domain.Model.ValueObjects;
using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace DevTech.SubscriptionPayment.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySubscriptionPaymentConfiguration(this ModelBuilder builder)
    {
        // Subscription Plan
        builder.Entity<SubscriptionPlan>().HasKey(p => p.Id);
        builder.Entity<SubscriptionPlan>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<SubscriptionPlanId, int>(id => id.Value,
                value => new SubscriptionPlanId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<SubscriptionPlan>().Property(p => p.Name).IsRequired().HasMaxLength(60);
        builder.Entity<SubscriptionPlan>().OwnsOne(p => p.Price, price =>
        {
            price.WithOwner().HasForeignKey("Id");
            price.Property(m => m.Amount).HasColumnName("price").HasPrecision(10, 2).IsRequired();
            price.Property(m => m.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();
        });
        builder.Entity<SubscriptionPlan>().Property(p => p.Credits).IsRequired();
        builder.Entity<SubscriptionPlan>().Property(p => p.DurationDays).IsRequired();

        // Subscription
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id)
            .HasConversion(new StronglyTypedIdConverter<SubscriptionId, int>(id => id.Value,
                value => new SubscriptionId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.CustomerId).IsRequired();
        builder.Entity<Subscription>().Property(s => s.PlanId)
            .HasConversion(new StronglyTypedIdConverter<SubscriptionPlanId, int>(id => id.Value,
                value => new SubscriptionPlanId(value)))
            .IsRequired();
        builder.Entity<Subscription>().Property(s => s.StartDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.ExpirationDate).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Credits).IsRequired();
        builder.Entity<Subscription>().HasIndex(s => s.CustomerId);
        // SUBSCRIPTION_PLANS ||--o{ SUBSCRIPTIONS : defines (database diagram, report section 4.8.1).
        // The aggregate references the plan only by id; a plan with subscriptions cannot be deleted.
        builder.Entity<Subscription>()
            .HasOne<SubscriptionPlan>()
            .WithMany()
            .HasForeignKey(s => s.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Payment
        builder.Entity<Payment>().HasKey(p => p.Id);
        builder.Entity<Payment>().Property(p => p.Id)
            .HasConversion(new StronglyTypedIdConverter<PaymentId, int>(id => id.Value, value => new PaymentId(value)))
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Entity<Payment>().Property(p => p.CustomerId).IsRequired();
        builder.Entity<Payment>().Property(p => p.OrderId).IsRequired();
        builder.Entity<Payment>().OwnsOne(p => p.Amount, amount =>
        {
            amount.WithOwner().HasForeignKey("Id");
            amount.Property(m => m.Amount).HasColumnName("amount").HasPrecision(10, 2).IsRequired();
            amount.Property(m => m.Currency).HasColumnName("currency").HasMaxLength(3).IsRequired();
        });
        builder.Entity<Payment>().Property(p => p.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Entity<Payment>().Property(p => p.TransactionId).HasMaxLength(100);
        builder.Entity<Payment>().Property(p => p.ProcessedAt);
        builder.Entity<Payment>().HasIndex(p => p.OrderId);
    }
}
