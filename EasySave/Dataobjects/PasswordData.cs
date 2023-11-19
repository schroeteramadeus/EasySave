namespace EasySave.Dataobjects
{
    using System.Security.Cryptography;

    /// <summary>
    /// Data class for password related meta data
    /// </summary>
    public class PasswordData
    {
        /// <summary>
        /// Used iteration count for hashing
        /// </summary>
        public int Iterations { get; private set; }
        /// <summary>
        /// Used salt for hashing
        /// </summary>
        public byte[] Salt { get; private set; }
        /// <summary>
        /// Used hash algorithm
        /// </summary>
        public HashAlgorithm HashAlgorithm { get; private set; }

        public PasswordData(byte[] salt, HashAlgorithm hashAlgorithm, int iterations)
        {
            this.Salt = salt;
            this.HashAlgorithm = hashAlgorithm;
            this.Iterations = iterations;
        }
    }
}
