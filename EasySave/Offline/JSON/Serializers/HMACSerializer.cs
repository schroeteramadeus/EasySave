using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Offline.JSON.Serializers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Newtonsoft.Json;

    public class HMACSerializer : JsonConverter<HMAC>
    {
        public override void WriteJson(JsonWriter writer, HMAC value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetType().Name);
        }

        public override HMAC ReadJson(JsonReader reader, Type objectType, HMAC existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return HMAC.Create(s);
        }
    }
    /*
     using System;
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    using EasySave.Dataobjects;

    public class HMACDataSerializer : JsonConverter<HMACData>
    {
        public override void WriteJson(JsonWriter writer, HMACData value, JsonSerializer serializer)
        {
            writer.WritePropertyName(nameof(value.Data));
            writer.WriteValue(value.Data);
            writer.WritePropertyName(nameof(value.HashAlgorithm));
            writer.WriteValue(value.HashAlgorithm.GetType().Name);
        }

        public override HMACData ReadJson(JsonReader reader, Type objectType, HMACData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new HMACData((string)reader.Value, HMAC.Create((string)reader.Value));
        }
    }
     */
}
