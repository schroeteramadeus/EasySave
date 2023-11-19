namespace EasySave.Crypto
{
    using System.Security.Cryptography;

    public class Password
    {
        /// <summary>
        /// The iteration count used
        /// </summary>
        public int Iterations { get; private set; }
        /// <summary>
        /// The salt used
        /// </summary>
        public byte[] Salt { get; private set; }
        /// <summary>
        /// The calculated hash
        /// </summary>
        public byte[] PasswordHash { get; private set; }
        /// <summary>
        /// The hash algorithm used
        /// </summary>
        public HashAlgorithm HashAlgorithm { get; private set; }

        /// <summary>
        /// Creates a new secure hash from already calculated values (see <see cref="Create(byte[], int, byte[], HashAlgorithm)"/>, <see cref="Create(string, int, byte[], HashAlgorithm)"/> or <see cref="Create(string, int, int, HashAlgorithm)"/> for non-hashed data)
        /// </summary>
        /// <param name="salt">The salt used for hashing</param>
        /// <param name="passwordHash">The hash</param>
        /// <param name="hashAlgorithm">The hash algorithm used</param>
        /// <param name="iterations">The count of hash iterations</param>
        public Password(byte[] salt, byte[] passwordHash, HashAlgorithm hashAlgorithm, int iterations)
        {
            this.Salt = salt;
            this.PasswordHash = passwordHash;
            this.HashAlgorithm = hashAlgorithm;
            this.Iterations = iterations;
        }
        /// <summary>
        /// Creates a new <see cref="Password"/>
        /// </summary>
        /// <param name="password">The password data</param>
        /// <param name="iterations">The iteration count for hashing</param>
        /// <param name="salt">The salt for hashing</param>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <returns>The <see cref="Password"/></returns>
        public static Password Create(byte[] password, int iterations, byte[] salt, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.Hash(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
        /// <summary>
        /// Creates a new <see cref="Password"/>
        /// </summary>
        /// <param name="password">The password string</param>
        /// <param name="iterations">The iteration count for hashing</param>
        /// <param name="saltLength">The salt length for hashing (creates a random sequence), see <see cref="Salt"/> after creation</param>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <returns>The <see cref="Password"/></returns>
        public static Password Create(string password, int iterations, int saltLength, HashAlgorithm hashAlgorithm)
        {
            byte[] salt = Hashing.GetRandomBytes(saltLength);
            byte[] hashed = Hashing.HashText(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
        /// <summary>
        /// Creates a new <see cref="Password"/>
        /// </summary>
        /// <param name="password">The password string</param>
        /// <param name="iterations">The iteration count for hashing</param>
        /// <param name="salt">The salt for hashing</param>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <returns>The <see cref="Password"/></returns>
        public static Password Create(string password, int iterations, byte[] salt, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.HashText(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
    }
}
