using System.Text.Json.Serialization;
using System.Text.Json;

namespace InternetShopWebApp.Models
{
    public class Base64ToByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string base64String = reader.GetString();
            return Convert.FromBase64String(base64String);
        }

        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
        {
            string base64String = Convert.ToBase64String(value);
            writer.WriteStringValue(base64String);
        }
    }
}
