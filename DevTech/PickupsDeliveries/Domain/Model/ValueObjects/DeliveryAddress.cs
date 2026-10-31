namespace DevTech.PickupsDeliveries.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing the validated address where a laundry order is picked up or delivered.
/// </summary>
/// <remarks>
///     An address is immutable and is always valid once created:
///     both the street and the city are required and must respect their maximum lengths.
///     Leading and trailing white spaces are removed before the values are stored.
/// </remarks>
public sealed record DeliveryAddress
{
    private const int StreetMaxLength = 255;
    private const int CityMaxLength = 100;

    /// <summary>
    ///     Initializes a new <see cref="DeliveryAddress" /> with the given street and city.
    /// </summary>
    /// <param name="street">The street of the address. Must be non-null, non-whitespace, and at most 255 characters.</param>
    /// <param name="city">The city of the address. Must be non-null, non-whitespace, and at most 100 characters.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="street" /> or <paramref name="city" /> is null, empty, whitespace,
    ///     or exceeds its maximum length.
    /// </exception>
    public DeliveryAddress(string street, string city)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be null, empty, or whitespace.", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be null, empty, or whitespace.", nameof(city));

        var trimmedStreet = street.Trim();
        var trimmedCity = city.Trim();

        if (trimmedStreet.Length > StreetMaxLength)
            throw new ArgumentException($"Street cannot be longer than {StreetMaxLength} characters.", nameof(street));

        if (trimmedCity.Length > CityMaxLength)
            throw new ArgumentException($"City cannot be longer than {CityMaxLength} characters.", nameof(city));

        Street = trimmedStreet;
        City = trimmedCity;
    }

    /// <summary>
    ///     Gets the street of the address.
    /// </summary>
    public string Street { get; }

    /// <summary>
    ///     Gets the city of the address.
    /// </summary>
    public string City { get; }

    /// <summary>
    ///     Returns the full address in the format "Street, City".
    /// </summary>
    /// <returns>The full address as a single text.</returns>
    public override string ToString() => $"{Street}, {City}";
}