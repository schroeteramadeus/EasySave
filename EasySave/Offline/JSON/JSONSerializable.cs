namespace EasySave.Offline.JSON
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;


    //TODO make interface / other architecture

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

        /// <summary>
        /// Creates a new <see cref="JSONDataObject"/> from the fields of the objects
        /// </summary>
        /// <returns>The data object</returns>
        public JSONDataObject CreateDataObject()
        {
            byte[] data = Encoding.Default.GetBytes(JsonConvert.SerializeObject(this, Formatting.None));
            return new JSONDataObject(this.GetType(), Encoding.Default, data);
        }
        /// <summary>
        /// Saves the data to the stream
        /// </summary>
        /// <param name="data">Data object to save</param>
        /// <param name="format">JSON format</param>
        /// <param name="s">The stream to save to</param>
        public static void Save(JSONDataObject data, Formatting format, Stream s)
        {
            using (StreamWriter writer = new StreamWriter(s))
            {
                writer.Write(Serialize(data, format));
                writer.Flush();
            }
        }
        /// <summary>
        /// Saves the data to the stream, with no extra formatting
        /// </summary>
        /// <param name="data">The data to save</param>
        /// <param name="s">The stream to save to</param>
        public static void Save(JSONDataObject data, Stream s)
        {
            Save(data, Formatting.None, s);
        }
        /// <summary>
        /// Loads data from a stream
        /// </summary>
        /// <param name="s">The stream to load from</param>
        /// <returns>The <see cref="JSONDataObject"/></returns>
        public static JSONDataObject Load(Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                JSONDataObject output = PartialDeserialize(reader.ReadToEnd());
                return output;
            }
        }
        /// <summary>
        /// Serializes the data to a saveable JSON string (JSON indented)
        /// </summary>
        /// <param name="data">The data to serialize</param>
        /// <returns>The string representation of the data</returns>
        public static string Serialize(JSONDataObject data)
        {
            return Serialize(data, Formatting.Indented);
        }
        /// <summary>
        /// Serializes the data to a saveable JSON string
        /// </summary>
        /// <param name="data">The data to serialize</param>
        /// <param name="format">The JSON format</param>
        /// <returns>The string representation of the data</returns>
        public static string Serialize(JSONDataObject data, Formatting format)
        {
            return JsonConvert.SerializeObject(data, format, converters.ToArray());
        }
        /// <summary>
        /// Deserializes the string into a <see cref="JSONDataObject"/>
        /// </summary>
        /// <param name="data">The JSON-formatted data</param>
        /// <returns>The deserialized <see cref="JSONDataObject"/></returns>
        public static JSONDataObject PartialDeserialize(string data)
        {
            return JsonConvert.DeserializeObject<JSONDataObject>(data, converters.ToArray());
        }
        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <typeparam name="T">The data type to convert to</typeparam>
        /// <param name="data">The data</param>
        /// <param name="password">The password (if needed)</param>
        /// <returns>The object of the given type</returns>
        /// <exception cref="ArgumentException">If the data type does not match the given type</exception>
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
        /// <summary>
        /// Deserializes the data, with no password
        /// </summary>
        /// <typeparam name="T">The data type to convert to</typeparam>
        /// <param name="data">The data</param>
        /// <returns>The object of the given type</returns>
        /// <exception cref="ArgumentException">If the data is encrypted</exception>
        public static T Deserialize<T>(JSONDataObject data) where T : JSONSerializable
        {
            if (!data.Encrypted)
            {
                return Deserialize<T>(data, null);
            }
            else
                throw new ArgumentException("Data is encrypted");
        }
        /// <summary>
        /// Deserializes the data, with no password
        /// </summary>
        /// <typeparam name="T">The data type to convert to</typeparam>
        /// <param name="data">The JSON string representation of the data</param>
        /// <returns>The object of the given type</returns>
        public static T Deserialize<T>(string data) where T : JSONSerializable
        {
            return Deserialize<T>(PartialDeserialize(data));
        }
        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <typeparam name="T">The data type to convert to</typeparam>
        /// <param name="data">The JSON string representation of the data</param>
        /// <param name="password">The password to use for decryption</param>
        /// <returns>The object of the given type</returns>
        public static T Deserialize<T>(string data, string password) where T : JSONSerializable
        {
            return Deserialize<T>(PartialDeserialize(data), password);
        }
    }
}
