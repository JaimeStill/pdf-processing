using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pdf.Api.Converters;
public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.FromDateTime(
            DateTime.Parse(
                reader.GetString()
                ?? DateTime.MinValue.ToString()
            )
        );
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}