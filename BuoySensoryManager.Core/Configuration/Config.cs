using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace BuoySensorManager.Core.Configuration
{
    public class Config : IConfig
    {
        public const string FilePath = "config.json";
        //
        //  I know IConfiguration is meant to be read only by default.
        //  This is me having some fun with boundaries.
        //  Using a text file of some sort allows transportable config
        //  in addition if the database is lost configuration will persist.
        //
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IPAddress? buoySensorEcbAddress = null;
        public IPAddress BuoySensorEcbAddress
        {
            get
            {
                string value = _configuration.GetValue(nameof(BuoySensorEcbAddress), "127.0.0.255");
                buoySensorEcbAddress ??= IPAddress.Parse(value);
                return buoySensorEcbAddress!;
            }
            set
            {
                SetValue(nameof(BuoySensorEcbAddress), value.ToString());
                buoySensorEcbAddress = value;
            }
        }

        private int? buoySensorEcbPort = null;
        public int BuoySensorEcbPort
        {
            get
            {
                buoySensorEcbPort ??= _configuration.GetValue(nameof(BuoySensorEcbPort), 9000);
                return buoySensorEcbPort.Value;
            }
            set
            {
                SetValue(nameof(BuoySensorEcbPort), value);
                buoySensorEcbPort = value;
            }
        }

        private int? buoySensorEcbPortCount = null;
        public int BuoySensorEcbPortCount
        {
            get
            {
                buoySensorEcbPortCount ??= _configuration.GetValue(nameof(BuoySensorEcbPortCount), 4);
                return buoySensorEcbPortCount.Value;
            }
            set
            {
                SetValue(nameof(BuoySensorEcbPortCount), value);
                buoySensorEcbPortCount = value;
            }
        }

        private int? buoyPacketPurgeInterval = null;
        public int BuoyPacketPurgeInterval
        {
            get
            {
                buoyPacketPurgeInterval ??= _configuration.GetValue(nameof(BuoyPacketPurgeInterval), 5);
                return buoyPacketPurgeInterval.Value;
            }
            set
            {
                SetValue(nameof(BuoyPacketPurgeInterval), value);
                buoyPacketPurgeInterval = value;
            }
        }

        private int? buoyPacketRetryInterval = null;
        public int BuoyPacketRetryInterval
        {
            get
            {
                buoyPacketRetryInterval ??= _configuration.GetValue(nameof(BuoyPacketRetryInterval), 5);
                return buoyPacketRetryInterval.Value;
            }
            set
            {
                SetValue(nameof(BuoyPacketRetryInterval), value);
                buoyPacketRetryInterval = value;
            }
        }

        private readonly Dictionary<int, string> portNames = [];

        public string GetBuoyName(int portNumber)
        {
            if (portNames.TryGetValue(portNumber, out string? name))
            {
                return name;
            }

            string portKey = GetPortKey(portNumber);
            name = _configuration.GetValue(portKey, "Not Set");

            portNames[portNumber] = name;

            return name;
        }

        public void SetBuoyName(int portNumber, string name)
        {
            string portKey = GetPortKey(portNumber);
            portNames[portNumber] = name;
            SetValue(portKey, name);
        }

        private static string GetPortKey(int portNumber) => $"Port{portNumber}Name";

        private static void SetValue(string key, string value)
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

        private static void SetValue(string key, int value)
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

        private static JsonElement GetDocumentRoot()
        {
            string json;

            if (File.Exists(FilePath))
            {
                json = File.ReadAllText(FilePath);
            }
            else
            {
                json = "{}";
            }

            var doc = JsonDocument.Parse(json);
            return doc.RootElement.Clone();
        }

        private static void WriteDocument(MemoryStream stream)
        {
            File.WriteAllText(FilePath, System.Text.Encoding.UTF8.GetString(stream.ToArray()));
        }
    }
}
