namespace EasySave.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    //TODO make use of the salt

    //adapted from https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
    public static class Encryption
    {
        /// <summary>
        /// Encrypts the data
        /// </summary>
        /// <param name="data">The data to encrypt</param>
        /// <param name="algorithm">The encryption algorithm to use</param>
        /// <param name="password">The password to use as key for encryption</param>
        /// <param name="iv">The IV-vector for encryption</param>
        /// <param name="salt">The salt for the encryption</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encrypt(byte[] data, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same salt and IV values can be used when decrypting.  
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(password.PasswordHash, iv))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        byte[] cipherTextBytes = memoryStream.ToArray();
                        memoryStream.Close();
                        cryptoStream.Close();
                        return cipherTextBytes;
                    }
                }
            }
        }
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="algorithm">The encryption algorithm to use</param>
        /// <param name="password">The password to use as key for encryption</param>
        /// <param name="iv">The IV-vector for encryption</param>
        /// <param name="salt">The salt for the encryption</param>
        /// <param name="fromEncoding">The encoding of the text, base64 if null</param>
        /// <returns>The base64 representation of the encrypted text</returns>
        public static string Encrypt(string text, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt, Encoding fromEncoding = null)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same salt and IV values can be used when decrypting.  
            byte[] plainTextBytes;
            if (fromEncoding != null)
            {
                plainTextBytes = fromEncoding.GetBytes(text);
            }
            else
            {
                plainTextBytes = Convert.FromBase64String(text);
            }

            return Convert.ToBase64String(Encrypt(plainTextBytes,algorithm, password, iv, salt));
        }
        /// <summary>
        /// Decrypts the data
        /// </summary>
        /// <param name="data">The data to decrypt</param>
        /// <param name="algorithm">The encryption algorithm used</param>
        /// <param name="password">The password to use as key for decryption</param>
        /// <param name="iv">The IV-vector for decryption</param>
        /// <param name="salt">The salt for the decryption</param>
        /// <returns>The non-encrypted data</returns>
        public static byte[] Decrypt(byte[] data, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            // Get the complete stream of bytes that represent:
            // [n bytes of CipherText]
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(password.PasswordHash, iv))
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = new byte[data.Length];
                        var decryptedByteCount = data.Length;

                        try
                        {
                            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        }
                        catch(System.Security.Cryptography.CryptographicException ex)
                        {
                            //probably wrong password
                        }
                        memoryStream.Close();
                        cryptoStream.Close();
                        //trim trailing 0's
                        return plainTextBytes[0..decryptedByteCount];
                    }
                }
            }
        }
        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="text">The text to decrypt</param>
        /// <param name="algorithm">The encryption algorithm used</param>
        /// <param name="password">The password to use as key for decryption</param>
        /// <param name="iv">The IV-vector for decryption</param>
        /// <param name="salt">The salt for the decryption</param>
        /// <param name="toEncoding">The encoding for the resulting string, base64 if null</param>
        /// <returns>The decrypted string</returns>
        public static string Decrypt(string text, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt, Encoding toEncoding = null)
        {
            // Get the complete stream of bytes that represent:
            // [n bytes of CipherText]
            byte[] cipherTextBytes = Convert.FromBase64String(text);

            cipherTextBytes = Decrypt(cipherTextBytes,algorithm, password, iv, salt);
            string output;
            if (toEncoding != null)
            {
                output = toEncoding.GetString(cipherTextBytes);
            }
            else
            {
                output = Convert.ToBase64String(cipherTextBytes);
            }

            return output;
        }
        /// <summary>
        /// Encrypts a base64 string
        /// </summary>
        /// <param name="text">The base64 string representation of the data</param>
        /// <param name="algorithm">The encryption algorithm to use</param>
        /// <param name="password">The password to use as key for encryption</param>
        /// <param name="iv">The IV-vector for encryption</param>
        /// <param name="salt">The salt for the encryption</param>
        /// <returns>The encrypted base64 string</returns>
        public static string EncryptBase64(string text, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            return Encrypt(text, algorithm, password, iv, salt);
        }
        /// <summary>
        /// Decrypts a base64 string
        /// </summary>
        /// <param name="text">The base64 string representation of the encrypted data</param>
        /// <param name="algorithm">The encryption algorithm used</param>
        /// <param name="password">The password to use as key for decryption</param>
        /// <param name="iv">The IV-vector for decryption</param>
        /// <param name="salt">The salt for the decryption</param>
        /// <returns>The non-encrypted base64 string</returns>
        public static string DecryptBase64(string text, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            return Decrypt(text, algorithm, password, iv, salt);
        }

    }
}
