namespace EasySave.Crypto
{
    using System.Security.Cryptography;

    public class Password
    {
        public int Iterations { get; private set; }
        public byte[] Salt { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public HashAlgorithm HashAlgorithm { get; private set; }

        public Password(byte[] salt, byte[] passwordHash, HashAlgorithm hashAlgorithm, int iterations)
        {
            this.Salt = salt;
            this.PasswordHash = passwordHash;
            this.HashAlgorithm = hashAlgorithm;
            this.Iterations = iterations;
        }
        public static Password Create(byte[] password, int iterations, byte[] salt, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.Hash(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
        public static Password Create(string password, int iterations, int saltLength, HashAlgorithm hashAlgorithm)
        {
            byte[] salt = Hashing.GetRandomBytes(saltLength);
            byte[] hashed = Hashing.HashText(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
        public static Password Create(string password, int iterations, byte[] salt, HashAlgorithm hashAlgorithm)
        {
            byte[] hashed = Hashing.HashText(hashAlgorithm, password, iterations, salt);
            return new Password(salt, hashed, hashAlgorithm, iterations);
        }
    }
}
