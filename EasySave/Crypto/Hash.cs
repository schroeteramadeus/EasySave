namespace EasySave.Crypto
{
    using System.Security.Cryptography;

    public class Hash
    {
        public byte[] HashData { get; private set; }
        public HashAlgorithm HashAlgorithm { get; private set; }

        public Hash(byte[] hashData, HashAlgorithm hashAlgorithm)
        {
            this.HashData = hashData;
            this.HashAlgorithm = hashAlgorithm;
        }

        public static Hash Create(string input, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.HashText(hashAlgorithm, input, 1, new byte[0]);
            return new Hash(hashed, hashAlgorithm);
        }
        public static Hash Create(byte[] input, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.Hash(hashAlgorithm, input, 1, new byte[0]);
            return new Hash(hashed, hashAlgorithm);
        }
    }
}
