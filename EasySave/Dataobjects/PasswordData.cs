namespace EasySave.Dataobjects
{
    using System.Security.Cryptography;

    public class PasswordData
    {
        public int Iterations { get; private set; }
        public byte[] Salt { get; private set; }
        public HashAlgorithm HashAlgorithm { get; private set; }

        public PasswordData(byte[] salt, HashAlgorithm hashAlgorithm, int iterations)
        {
            this.Salt = salt;
            this.HashAlgorithm = hashAlgorithm;
            this.Iterations = iterations;
        }
    }
}
