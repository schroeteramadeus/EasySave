namespace EasySave.Dataobjects 
{
    using System.Security.Cryptography;

    public class EncryptionData
    {
        public HMACData HMAC { get; private set; }
        public byte[] Salt { get; private set; }
        public byte[] IV { get; private set; }
        public CipherMode CipherMode { get; private set; }
        public PaddingMode PaddingMode { get; private set; }
        public SymmetricAlgorithm Algorithm { get; private set; }

        public EncryptionData(byte[] salt, byte[] iv, CipherMode cipherMode, PaddingMode paddingMode, SymmetricAlgorithm algorithm, HMACData hmac)
        {
            this.Salt = salt;
            this.IV = iv;
            this.CipherMode = cipherMode;
            this.PaddingMode = paddingMode;
            this.Algorithm = algorithm;
            this.HMAC = hmac;
        }
    }
}
