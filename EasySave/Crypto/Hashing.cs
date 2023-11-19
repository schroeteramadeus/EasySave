namespace EasySave.Crypto
{
    using System;
    using System.Security.Cryptography;

    public static class Hashing
    {
        private static System.Security.Cryptography.RNGCryptoServiceProvider _provider = new System.Security.Cryptography.RNGCryptoServiceProvider();

        /// <summary>
        /// Generates a random byte sequence
        /// </summary>
        /// <param name="length">The length of the byte sequence</param>
        /// <returns>A random byte sequence</returns>
        /// <exception cref="ArgumentException">If the length is 0 or less</exception>
        public static byte[] GetRandomBytes(int length)
        {
            if (length > 0)
            {
                byte[] byteArr = new byte[length];
                _provider.GetBytes(byteArr);
                return byteArr;
            }
            else
                throw new ArgumentException("The length given is 0 or less.");
        }
        /// <summary>
        /// Creates a once-hashed hash of the data
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <param name="bytes">The data to hash</param>
        /// <returns>The once-hashed data</returns>
        public static byte[] HashByteArray(HashAlgorithm hashAlgorithm, byte[] bytes)
        {
            return hashAlgorithm.ComputeHash(bytes);
        }
        /// <summary>
        /// Creates a hash of the data
        /// </summary>
        /// <param name="hashFunctionToUse">The hash algorithm to use</param>
        /// <param name="data">The data to hash</param>
        /// <param name="iterationCount">The hash iterations</param>
        /// <param name="salt">The salt for the iterations</param>
        /// <returns>The hashed data</returns>
        public static byte[] Hash(Func<byte[], byte[]> hashFunctionToUse, byte[] data, int iterationCount, byte[] salt)
        {
            byte[] hashed = data;

            for (int x = 0; x < iterationCount; x++)
            {
                byte[] newHash = new byte[salt.Length + hashed.Length];
                Array.Copy(salt, newHash, salt.Length);
                Array.Copy(hashed, 0, newHash, salt.Length, hashed.Length);
                hashed = hashFunctionToUse(newHash);
            }
            return hashed;
        }
        /// <summary>
        /// Creates a hash of the data
        /// </summary>
        /// <param name="hashFunctionToUse">The hash algorithm to use</param>
        /// <param name="data">The data to hash</param>
        /// <param name="iterationCount">The hash iterations</param>
        /// <param name="salt">The salt for the iterations</param>
        /// <returns>The hashed data</returns>
        public static byte[] Hash(HashAlgorithm hashFunctionToUse, byte[] data, int iterationCount, byte[] salt)
        {
            byte[] hashed = data;

            return Hash(hashFunctionToUse.ComputeHash, hashed, iterationCount, salt);
        }
        /// <summary>
        /// Creates a hash of the data
        /// </summary>
        /// <param name="hashFunctionToUse">The hash algorithm to use</param>
        /// <param name="text">The text to hash</param>
        /// <param name="iterationCount">The hash iterations</param>
        /// <param name="salt">The salt for the iterations</param>
        /// <returns>The hashed data</returns>
        public static byte[] HashText(Func<byte[], byte[]> hashFunctionToUse, string text, int iterationCount, byte[] salt)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(text);

            return Hash(hashFunctionToUse, data, iterationCount, salt);
        }
        /// <summary>
        /// Creates a hash of the data
        /// </summary>
        /// <param name="hashFunctionToUse">The hash algorithm to use</param>
        /// <param name="text">The text to hash</param>
        /// <param name="iterationCount">The hash iterations</param>
        /// <param name="salt">The salt for the iterations</param>
        /// <returns>The hashed data</returns>
        public static byte[] HashText(HashAlgorithm hashFunctionToUse, string text, int iterationCount, byte[] salt)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(text);
            return Hash(hashFunctionToUse, data, iterationCount, salt);
        }
    }
}
