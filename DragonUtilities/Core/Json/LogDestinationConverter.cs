using System.Text.Json;
using System.Text.Json.Serialization;
using DragonUtilities.Core.Logging;
using DragonUtilities.Interfaces;

namespace DragonUtilities.Core.Json;

public class LogDestinationConverter : JsonConverter<ILogDestination>
{
    public override ILogDestination Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token");

        var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        if (!root.TryGetProperty("Type", out var typeElement))
            throw new JsonException("Missing 'Type' property for ILogDestination");

        var type = typeElement.GetString();
        if (type == null)
            throw new JsonException("'Type' property cannot be null");

        return (type switch
        {
            nameof(FileLogDestination) => JsonSerializer.Deserialize<FileLogDestination>(root.GetRawText(), options),
            nameof(ConsoleLogDestination) => JsonSerializer.Deserialize<ConsoleLogDestination>(root.GetRawText(), options),
            _ => throw new JsonException($"Unknown ILogDestination type: {type}")
        })!;
    }

    public override void Write(Utf8JsonWriter writer, ILogDestination value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case FileLogDestination fileLog:
                writer.WriteStartObject();
                writer.WriteString("Type", nameof(FileLogDestination));
                writer.WriteString("FilePath", fileLog.FilePath);
                writer.WriteEndObject();
                break;

            case ConsoleLogDestination:
                writer.WriteStartObject();
                writer.WriteString("Type", nameof(ConsoleLogDestination));
                writer.WriteEndObject();
                break;

            default:
                throw new JsonException($"Unsupported ILogDestination type: {value.GetType()}");
        }
    }
}