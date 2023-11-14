namespace EasySave.Offline.JSON.Serializers
{
    using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;

    public class HashAlgorithmSerializer : JsonConverter<HashAlgorithm>
    {
        public override void WriteJson(JsonWriter writer, HashAlgorithm value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetType().BaseType.Name);
        }

        public override HashAlgorithm ReadJson(JsonReader reader, Type objectType, HashAlgorithm existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            HashAlgorithm h = HashAlgorithm.Create(s);
            return h;
        }
    }
}
