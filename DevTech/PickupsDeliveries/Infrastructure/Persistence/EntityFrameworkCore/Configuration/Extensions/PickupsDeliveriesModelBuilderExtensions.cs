using DevTech.PickupsDeliveries.Domain.Model.Aggregates;
using DevTech.PickupsDeliveries.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevTech.PickupsDeliveries.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Model builder extensions for the Pickups and Deliveries bounded context.
/// </summary>
/// <remarks>
///     Keeps the EF Core mapping of this bounded context in its own infrastructure layer.
///     The shared AppDbContext only calls <see cref="ApplyPickupsDeliveriesConfiguration" />.
///     The table (<c>pickup_deliveries</c>) and column names are converted to snake_case afterwards by
///     <c>UseSnakeCaseNamingConvention</c>, except the address columns, which are named here.
/// </remarks>
public static class PickupsDeliveriesModelBuilderExtensions
{
    private const int ServiceTypeMaxLength = 20;
    private const int StatusMaxLength = 20;
    private const string AddressStreetColumnName = "address_street";
    private const string AddressCityColumnName = "address_city";

    /// <summary>
    ///     Applies the EF Core mapping for the PickupDelivery aggregate.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item><description>DeliveryAddress is stored as a complex property in the same table.</description></item>
    ///         <item><description>ServiceType and PickupDeliveryStatus are stored as text, not as numbers.</description></item>
    ///         <item><description>The pair (OrderId, Type) has a unique index: one pickup and one delivery per order.</description></item>
    ///     </list>
    /// </remarks>
    /// <param name="builder">The model builder for the database context</param>
    public static void ApplyPickupsDeliveriesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<PickupDelivery>().HasKey(pickupDelivery => pickupDelivery.Id);
        builder.Entity<PickupDelivery>().Property(pickupDelivery => pickupDelivery.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Entity<PickupDelivery>().Property(pickupDelivery => pickupDelivery.OrderId)
            .IsRequired();

        builder.Entity<PickupDelivery>().ComplexProperty(pickupDelivery => pickupDelivery.Address, address =>
        {
            address.IsRequired();
            address.Property(deliveryAddress => deliveryAddress.Street)
                .HasColumnName(AddressStreetColumnName)
                .HasMaxLength(DeliveryAddress.StreetMaxLength)
                .IsRequired();
            address.Property(deliveryAddress => deliveryAddress.City)
                .HasColumnName(AddressCityColumnName)
                .HasMaxLength(DeliveryAddress.CityMaxLength)
                .IsRequired();
        });

        builder.Entity<PickupDelivery>().Property(pickupDelivery => pickupDelivery.Type)
            .HasConversion<string>()
            .HasMaxLength(ServiceTypeMaxLength)
            .IsRequired();

        builder.Entity<PickupDelivery>().Property(pickupDelivery => pickupDelivery.Status)
            .HasConversion<string>()
            .HasMaxLength(StatusMaxLength)
            .IsRequired();

        builder.Entity<PickupDelivery>()
            .HasIndex(pickupDelivery => new { pickupDelivery.OrderId, pickupDelivery.Type })
            .IsUnique();
    }
}
