using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevTech.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

/// <summary>
///     Stores <see cref="DateOnly" /> values as <see cref="DateTime" />.
/// </summary>
/// <remarks>
///     The MySQL provider (MySql.EntityFrameworkCore) reads DATE columns as <see cref="DateTime" /> and cannot
///     cast them to <see cref="DateOnly" />, so every <see cref="DateOnly" /> property goes through this converter.
/// </remarks>
public class DateOnlyToDateTimeConverter() : ValueConverter<DateOnly, DateTime>(
    date => date.ToDateTime(TimeOnly.MinValue),
    dateTime => DateOnly.FromDateTime(dateTime));
