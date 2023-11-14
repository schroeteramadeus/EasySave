namespace EasySave.Offline.JSON.Serializers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Newtonsoft.Json;

    public class SymmetricAlgorithmSerializer : JsonConverter<SymmetricAlgorithm>
    {
        public override void WriteJson(JsonWriter writer, SymmetricAlgorithm value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetType().BaseType.Name);
        }

        public override SymmetricAlgorithm ReadJson(JsonReader reader, Type objectType, SymmetricAlgorithm existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return SymmetricAlgorithm.Create(s);
        }
    }
}
