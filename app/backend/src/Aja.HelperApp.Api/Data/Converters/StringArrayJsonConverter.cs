using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Aja.HelperApp.Api.Data.Converters;

public sealed class StringArrayJsonConverter() : ValueConverter<string[], string>(
    values => JsonSerializer.Serialize(values, (JsonSerializerOptions?)null),
    value => JsonSerializer.Deserialize<string[]>(value, (JsonSerializerOptions?)null) ?? Array.Empty<string>())
{
    public static ValueComparer<string[]> Comparer { get; } = new(
        (left, right) => ReferenceEquals(left, right) ||
            (left != null && right != null && left.SequenceEqual(right)),
        values => values.Aggregate(0, (hash, value) => HashCode.Combine(hash, value)),
        values => values.ToArray());
}
