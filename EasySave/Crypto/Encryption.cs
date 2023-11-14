namespace EasySave.Crypto
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    //adapted from https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
    public static class Encryption
    {
        public static byte[] Encrypt(byte[] plainTextBytes, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(password.PasswordHash, iv))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        byte[] cipherTextBytes = memoryStream.ToArray();
                        memoryStream.Close();
                        cryptoStream.Close();
                        return cipherTextBytes;
                    }
                }
            }
        }
        public static string Encrypt(string plainText, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt, Encoding fromEncoding = null)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            byte[] plainTextBytes;
            if (fromEncoding != null)
            {
                plainTextBytes = fromEncoding.GetBytes(plainText);
            }
            else
            {
                plainTextBytes = Convert.FromBase64String(plainText);
            }

            return Convert.ToBase64String(Encrypt(plainTextBytes,algorithm, password, iv, salt));
        }
        public static byte[] Decrypt(byte[] cipherText, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            // Get the complete stream of bytes that represent:
            // [n bytes of CipherText]
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(password.PasswordHash, iv))
            {
                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainTextBytes = new byte[cipherText.Length];
                        var decryptedByteCount = cipherText.Length;

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
        public static string Decrypt(string cipherText, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt, Encoding toEncoding = null)
        {
            // Get the complete stream of bytes that represent:
            // [n bytes of CipherText]
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

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

        public static string EncryptBase64(string plainText, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            return Encrypt(plainText, algorithm, password, iv, salt);
        }
        public static string DecryptBase64(string cipherText, SymmetricAlgorithm algorithm, Password password, byte[] iv, byte[] salt)
        {
            return Decrypt(cipherText, algorithm, password, iv, salt);
        }

    }
}
