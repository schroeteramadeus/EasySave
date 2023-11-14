namespace EasySave.Offline.JSON
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;

    public abstract class JSONSerializable
    {
        private static List<JsonConverter> converters = new List<JsonConverter>()
        {
            new Newtonsoft.Json.Converters.StringEnumConverter(),
            new Serializers.HMACSerializer(),
            new Serializers.EncodingSerializer(),
            new Serializers.HashAlgorithmSerializer(),
            new Serializers.SymmetricAlgorithmSerializer(),
        };

        public JSONDataObject CreateDataObject()
        {
            byte[] data = Encoding.Default.GetBytes(JsonConvert.SerializeObject(this, Formatting.None));
            return new JSONDataObject(this.GetType(), Encoding.Default, data);
        }

        public static void Save(JSONDataObject data, Formatting format, Stream s)
        {
            using (StreamWriter writer = new StreamWriter(s))
            {
                writer.Write(Serialize(data, format));
                writer.Flush();
            }
        }
        public static void Save(JSONDataObject data, Stream s)
        {
            Save(data, Formatting.None, s);
        }
        public static JSONDataObject Load(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                JSONDataObject output = PartialDeserialize(reader.ReadToEnd());
                return output;
            }
        }
        public static string Serialize(JSONDataObject data)
        {
            return Serialize(data, Formatting.Indented);
        }
        public static string Serialize(JSONDataObject data, Formatting format)
        {
            return JsonConvert.SerializeObject(data, format, converters.ToArray());
        }

        public static JSONDataObject PartialDeserialize(string data)
        {
            return JsonConvert.DeserializeObject<JSONDataObject>(data, converters.ToArray());
        }
        public static T Deserialize<T>(JSONDataObject data, string password) where T : JSONSerializable
        {
            if (typeof(T) == data.Type)
            {
                if (data.Encrypted)
                    data.Decrypt(password);
                if (data.Compressed)
                    data.Decompress();
                T obj = JsonConvert.DeserializeObject<T>(data.OriginalEncoding.GetString(data.Data));
                return obj;
            }
            else
                throw new ArgumentException("Could not convert type " + data.Type + " to object of type " + typeof(T));
        }
        public static T Deserialize<T>(JSONDataObject data) where T : JSONSerializable
        {
            if (!data.Encrypted)
            {
                return Deserialize<T>(data, null);
            }
            else
                throw new ArgumentException("Data is encrypted");
        }
        public static T Deserialize<T>(string data) where T : JSONSerializable
        {
            return Deserialize<T>(PartialDeserialize(data));
        }
        public static T Deserialize<T>(string data, string password) where T : JSONSerializable
        {
            return Deserialize<T>(PartialDeserialize(data), password);
        }
    }
}
