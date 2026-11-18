using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aja.HelperApp.Api.Contracts;

public sealed class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetDateTime().ToUniversalTime();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteStringValue(DateTime.SpecifyKind(value, DateTimeKind.Utc));
}
