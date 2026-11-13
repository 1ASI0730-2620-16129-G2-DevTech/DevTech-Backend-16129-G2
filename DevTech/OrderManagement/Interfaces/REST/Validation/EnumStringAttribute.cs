using System.ComponentModel.DataAnnotations;

namespace DevTech.OrderManagement.Interfaces.REST.Validation;

/// <summary>
///     Validates that a string carries the name of a member of an enum (case-insensitive).
///     Numeric strings are rejected on purpose: the API exposes enums by name only.
/// </summary>
/// <param name="enumType">The enum whose member names are accepted.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class EnumStringAttribute(Type enumType) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // A missing value is the job of [Required], not of this attribute.
        if (value is null) return ValidationResult.Success;

        var names = Enum.GetNames(enumType);
        if (value is string text && names.Contains(text, StringComparer.OrdinalIgnoreCase))
            return ValidationResult.Success;

        var members = validationContext.MemberName is null
            ? Array.Empty<string>()
            : new[] { validationContext.MemberName };

        return new ValidationResult(
            $"'{value}' is not a valid {enumType.Name}. Allowed values: {string.Join(", ", names)}.", members);
    }
}
