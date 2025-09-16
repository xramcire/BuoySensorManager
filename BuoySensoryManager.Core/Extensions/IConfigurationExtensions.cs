using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BuoySensorManager.Core.Extensions
{
    internal static class IConfigurationExtensions
    {
        //
        //  These arent really extensions but I want them to appear as one...
        //  I need to extend the IConfiguration interface to contain the name of the config file.
        //  Or something like that... This is a hack, but it works for now.
        //
        internal static void SetValue(this IConfiguration configuration, string key, string value)
        {
            var root = GetDocumentRoot();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

            writer.WriteStartObject();

            foreach (var property in root.EnumerateObject())
            {
                if (property.NameEquals(key))
                {
                    writer.WriteString(property.Name, value);
                }
                else
                {
                    property.WriteTo(writer);
                }
            }

            // If key doesn't exist, add it
            if (!root.TryGetProperty(key, out _))
            {
                writer.WriteString(key, value);
            }

            writer.WriteEndObject();
            writer.Flush();

            WriteDocument(stream);
        }

        internal static void SetValue(this IConfiguration configuration, string key, int value)
        {
            var root = GetDocumentRoot();

            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

            writer.WriteStartObject();

            foreach (var property in root.EnumerateObject())
            {
                if (property.NameEquals(key))
                {
                    writer.WriteNumber(property.Name, value);
                }
                else
                {
                    property.WriteTo(writer);
                }
            }

            if (!root.TryGetProperty(key, out _))
            {
                writer.WriteNumber(key, value);
            }

            writer.WriteEndObject();
            writer.Flush();

            WriteDocument(stream);
        }

        private const string FilePath = "appsettings.json";

        private static JsonElement GetDocumentRoot()
        {
            var json = File.ReadAllText(FilePath);
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }

        private static void WriteDocument(MemoryStream stream)
        {
            File.WriteAllText(FilePath, System.Text.Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}
