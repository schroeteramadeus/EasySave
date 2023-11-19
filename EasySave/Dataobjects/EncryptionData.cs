namespace EasySave.Dataobjects 
{
    using System.Security.Cryptography;

    /// <summary>
    /// Data class for encryption related meta data
    /// </summary>
    public class EncryptionData
    {
        /// <summary>
        /// The hmac for the data
        /// </summary>
        public HMACData HMAC { get; private set; }
        /// <summary>
        /// The salt used
        /// </summary>
        public byte[] Salt { get; private set; }
        /// <summary>
        /// The iv vector used
        /// </summary>
        public byte[] IV { get; private set; }
        /// <summary>
        /// The cipher mode used
        /// </summary>
        public CipherMode CipherMode { get; private set; }
        /// <summary>
        /// The padding mode used
        /// </summary>
        public PaddingMode PaddingMode { get; private set; }
        /// <summary>
        /// The encryption algorithm used
        /// </summary>
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
