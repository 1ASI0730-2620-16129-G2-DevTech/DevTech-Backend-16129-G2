namespace DevTech.Shared.Domain.Model.ValueObjects;

/// <summary>
///     Represents a monetary amount in a specific currency.
/// </summary>
/// <remarks>
///     Shared Kernel value object. It is immutable and self-validating:
///     the amount cannot be negative and the currency must be a 3-letter ISO 4217 code (e.g. PEN, USD).
/// </remarks>
public record Money
{
    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    private Money()
    {
        Currency = string.Empty;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Money" /> value object.
    /// </summary>
    /// <param name="amount">The monetary amount. Must not be negative.</param>
    /// <param name="currency">The ISO 4217 currency code.</param>
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");
        if (!IsValidCurrency(currency))
            throw new ArgumentException("Currency must be a 3-letter ISO 4217 code.", nameof(currency));
        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    /// <summary>
    ///     Gets the monetary amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    ///     Gets the ISO 4217 currency code.
    /// </summary>
    public string Currency { get; init; }

    /// <summary>
    ///     Checks whether the given currency code is a valid 3-letter code.
    /// </summary>
    /// <param name="currency">The currency code to check.</param>
    /// <returns><c>true</c> if the code is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidCurrency(string? currency)
    {
        return !string.IsNullOrWhiteSpace(currency) && currency.Length == 3 && currency.All(char.IsLetter);
    }

    /// <summary>
    ///     Adds another <see cref="Money" /> value with the same currency.
    /// </summary>
    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException("Cannot add money values with different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>
    ///     Multiplies the amount by a non-negative factor.
    /// </summary>
    public Money Multiply(decimal factor)
    {
        return new Money(Amount * factor, Currency);
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
