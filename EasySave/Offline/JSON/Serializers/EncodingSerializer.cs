namespace EasySave.Offline.JSON.Serializers
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// JSON Serializer for saving <see cref="Encoding"/> correctly
    /// </summary>
    public class EncodingSerializer : JsonConverter<Encoding>
    {
        public override void WriteJson(JsonWriter writer, Encoding value, JsonSerializer serializer)
        {
            writer.WriteValue(value.BodyName);
        }

        public override Encoding ReadJson(JsonReader reader, Type objectType, Encoding existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;

            return Encoding.GetEncoding(s);
        }
    }
}
