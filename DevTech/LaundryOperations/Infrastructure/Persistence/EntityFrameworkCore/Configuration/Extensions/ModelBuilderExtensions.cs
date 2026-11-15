using DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;
using DevTech.LaundryOperations.Domain.Model.Aggregates;
using DevTech.LaundryOperations.Domain.Model.Entities;
using DevTech.LaundryOperations.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
namespace DevTech.LaundryOperations.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyLaundryOperationsConfiguration(this ModelBuilder model)
    {
        var order = model.Entity<LaundryOrder>();
        var utcTimestampConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTimeOffset?, DateTime?>(
            value => value.HasValue ? (DateTime?)value.Value.UtcDateTime : null,
            value => value.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)) : null);
        order.Property(x => x.CreatedAt).HasConversion(utcTimestampConverter);
        order.Property(x => x.UpdatedAt).HasConversion(utcTimestampConverter);
        order.ToTable("laundry_orders");
        order.HasKey(x => x.Id);
        order.Property(x => x.Id).HasConversion(new StronglyTypedIdConverter<LaundryOrderId, Guid>(x => x.Value, x => new LaundryOrderId(x))).ValueGeneratedNever();
        order.Property(x => x.LaundryId).HasConversion(new StronglyTypedIdConverter<LaundryId, Guid>(x => x.Value, x => new LaundryId(x)));
        order.Property(x => x.WashingCycleId).HasConversion(
            x => x.HasValue ? (Guid?)x.Value.Value : null,
            x => x.HasValue ? new WashingCycleId(x.Value) : null);
        order.Property(x => x.ResourceId).HasConversion(
            x => x.HasValue ? (Guid?)x.Value.Value : null,
            x => x.HasValue ? new LaundryResourceId(x.Value) : null);
        order.Property(x => x.CurrentStage).HasConversion<string>().HasMaxLength(30);
        order.Property(x => x.Priority).HasConversion<string>().HasMaxLength(10);
        order.Property(x => x.ExpectedCompletion).HasConversion(
            x => x.HasValue ? (DateTime?)x.Value.UtcDateTime : null,
            x => x.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(x.Value, DateTimeKind.Utc)) : null);
        order.Property(x => x.Version).IsConcurrencyToken();
        order.HasIndex(x => x.OrderId).IsUnique();
        order.HasIndex(x => x.LaundryId);
        order.Ignore(x => x.DomainEvents);
        order.Ignore(x => x.IsReady);
        order.HasOne<WashingCycle>().WithMany().HasForeignKey(x => x.WashingCycleId).OnDelete(DeleteBehavior.Restrict);
        order.HasOne<LaundryResource>().WithMany().HasForeignKey(x => x.ResourceId).OnDelete(DeleteBehavior.Restrict);

        var resource = model.Entity<LaundryResource>();
        resource.Property(x => x.CreatedAt).HasConversion(utcTimestampConverter);
        resource.Property(x => x.UpdatedAt).HasConversion(utcTimestampConverter);
        resource.ToTable("laundry_resources");
        resource.HasKey(x => x.Id);
        resource.Property(x => x.Id).HasConversion(new StronglyTypedIdConverter<LaundryResourceId, Guid>(x => x.Value, x => new LaundryResourceId(x))).ValueGeneratedNever();
        resource.Property(x => x.LaundryId).HasConversion(new StronglyTypedIdConverter<LaundryId, Guid>(x => x.Value, x => new LaundryId(x)));
        resource.Property(x => x.Name).HasMaxLength(100).IsRequired();
        resource.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        resource.Property(x => x.Version).IsConcurrencyToken();
        resource.HasIndex(x => x.LaundryId);
        resource.HasIndex(x => x.AssignedOrderId).IsUnique();
        resource.Ignore(x => x.DomainEvents);
        resource.Ignore(x => x.IsAvailable);

        var cycle = model.Entity<WashingCycle>();
        cycle.ToTable("washing_cycles");
        cycle.HasKey(x => x.Id);
        cycle.Property(x => x.Id).HasConversion(new StronglyTypedIdConverter<WashingCycleId, Guid>(x => x.Value, x => new WashingCycleId(x))).ValueGeneratedNever();
        cycle.Property(x => x.Name).HasMaxLength(100).IsRequired();
        cycle.Property(x => x.CompatibleType).HasMaxLength(100).IsRequired();
    }
}

