using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeachBoard.IdentityService.WebApi;

public class CustomJsonStringEnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            return (T)Enum.Parse(typeToConvert, reader.GetString(), ignoreCase: true);
        }
        catch (ArgumentException ex)
        {
            throw new JsonException($"Error deserializing {typeToConvert.Name}: {ex.Message}");
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}