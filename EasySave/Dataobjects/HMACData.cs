namespace EasySave.Dataobjects
{
    using System.Security.Cryptography;
    using Newtonsoft.Json;

    /// <summary>
    /// Data class for hmac data
    /// </summary>
    public class HMACData
    {
        /// <summary>
        /// The hmac
        /// </summary>
        [JsonProperty]
        public byte[] Data { get; private set; }
        /// <summary>
        /// The hmac algorithm
        /// </summary>
        [JsonProperty]
        public HMAC HashAlgorithm { get; private set; }

        public HMACData(byte[] hmacdata,HMAC algorithm)
        {
            this.Data = hmacdata;
            this.HashAlgorithm = algorithm;
        }
    }
}
