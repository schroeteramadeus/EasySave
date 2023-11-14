namespace EasySave.Crypto
{
    using System;
    using System.Security.Cryptography;

    public static class Hashing
    {
        private static System.Security.Cryptography.RNGCryptoServiceProvider _provider = new System.Security.Cryptography.RNGCryptoServiceProvider();

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
        public static byte[] HashByteArray(HashAlgorithm hashAlgorithm, byte[] bytes)
        {
            return hashAlgorithm.ComputeHash(bytes);
        }
        public static byte[] Hash(Func<byte[], byte[]> hashFunctionToUse, byte[] textToEncrypt, int iterationCount, byte[] salt)
        {
            byte[] encryption = textToEncrypt;

            for (int x = 0; x < iterationCount; x++)
            {
                byte[] encrypt = new byte[salt.Length + encryption.Length];
                Array.Copy(salt, encrypt, salt.Length);
                Array.Copy(encryption, 0, encrypt, salt.Length, encryption.Length);
                encryption = hashFunctionToUse(encrypt);
            }
            return encryption;
        }
        public static byte[] Hash(HashAlgorithm hashFunctionToUse, byte[] textToEncrypt, int iterationCount, byte[] salt)
        {
            byte[] encryption = textToEncrypt;

            return Hash(hashFunctionToUse.ComputeHash, encryption, iterationCount, salt);
        }
        public static byte[] HashText(Func<byte[], byte[]> hashFunctionToUse, string textToEncrypt, int iterationCount, byte[] salt)
        {
            byte[] encryption = System.Text.Encoding.Default.GetBytes(textToEncrypt);

            return Hash(hashFunctionToUse, encryption, iterationCount, salt);
        }
        public static byte[] HashText(HashAlgorithm hashFunctionToUse, string textToEncrypt, int iterationCount, byte[] salt)
        {
            byte[] encryption = System.Text.Encoding.Default.GetBytes(textToEncrypt);
            return Hash(hashFunctionToUse, encryption, iterationCount, salt);
        }
    }
}
