namespace EasySave.Crypto
{
    using System.Security.Cryptography;

    /// <summary>
    /// Simple (low security) Hash
    /// </summary>
    /// <remarks>
    /// Do NOT use for high security applications, instead use <see cref="Password"/>
    /// </remarks>
    public class Hash
    {
        /// <summary>
        /// The raw hash (data)
        /// </summary>
        public byte[] HashData { get; private set; }
        /// <summary>
        /// The Hash algorythm used
        /// </summary>
        public HashAlgorithm HashAlgorithm { get; private set; }

        /// <summary>
        /// Creates a new hash from already calculated values (see <see cref="Create(byte[], HashAlgorithm)"/> or <see cref="Create(string, HashAlgorithm)"/> for non-hashed data)
        /// </summary>
        /// <param name="hashData">The already once-hashed data</param>
        /// <param name="hashAlgorithm">The hash algorithm used</param>
        public Hash(byte[] hashData, HashAlgorithm hashAlgorithm)
        {
            this.HashData = hashData;
            this.HashAlgorithm = hashAlgorithm;
        }
        /// <summary>
        /// Creates a new Hash
        /// </summary>
        /// <param name="input">The text to hash</param>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <returns>The resulting <see cref="Hash"/> (once-hashed)</returns>
        public static Hash Create(string input, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.HashText(hashAlgorithm, input, 1, new byte[0]);
            return new Hash(hashed, hashAlgorithm);
        }
        /// <summary>
        /// Creates a new Hash
        /// </summary>
        /// <param name="input">The data to hash</param>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <returns>The resulting <see cref="Hash"/> (once-hashed)</returns>
        public static Hash Create(byte[] input, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.Hash(hashAlgorithm, input, 1, new byte[0]);
            return new Hash(hashed, hashAlgorithm);
        }
    }
}
