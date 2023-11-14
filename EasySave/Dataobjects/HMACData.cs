namespace EasySave.Dataobjects
{
    using System.Security.Cryptography;
    using Newtonsoft.Json;
    public class HMACData
    {
        [JsonProperty]
        public byte[] Data { get; private set; }
        [JsonProperty]
        public HMAC HashAlgorithm { get; private set; }

        public HMACData(byte[] hmacdata,HMAC algorithm)
        {
            this.Data = hmacdata;
            this.HashAlgorithm = algorithm;
        }
    }
}
