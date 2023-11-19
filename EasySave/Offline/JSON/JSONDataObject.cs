namespace EasySave.Offline.JSON
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Newtonsoft.Json;
    using EasySave.Crypto;
    using EasySave.Offline.JSON.Serializers;
    using EasySave.Dataobjects;
    using System.Linq;
    using System.IO.Compression;

    public class JSONDataObject
    {
        private Compression _compression = new Compression();

        /// <summary>
        /// Type of the underlying data
        /// </summary>
        public Type Type { get; private set; }

        //[JsonConverter(typeof(EncodingSerializer))]
        /// <summary>
        /// Encoding of the original data
        /// </summary>
        public Encoding OriginalEncoding { get; private set; }

        /// <summary>
        /// Determines wether the current data is compressed
        /// </summary>
        public bool Compressed { get; private set; }
        /// <summary>
        /// Determines wether the current data is encrypted
        /// </summary>
        public bool Encrypted { get; private set; }
        /// <summary>
        /// The password related meta data
        /// </summary>
        public PasswordData PasswordData { get; private set; }
        /// <summary>
        /// The encryption related meta data
        /// </summary>
        public EncryptionData EncryptionData { get; private set; }
        /// <summary>
        /// The compression related meta data
        /// </summary>
        public CompressionData CompressionData { get; private set; }
        /// <summary>
        /// The underlying data
        /// </summary>
        public byte[] Data { get; private set; }

        public JSONDataObject(Type type, Encoding originalEncoding, byte[] data)
        {
            this.Initialize(type, originalEncoding, data);
        }
        [JsonConstructor]
        public JSONDataObject(Type type, bool compressed, Encoding originalEncoding, bool encrypted, EncryptionData encryptionData, CompressionData compressionData, PasswordData passwordData, byte[] data)
        {
            this.Initialize(type, originalEncoding, data);
            this.Compressed = compressed;
            this.Encrypted = encrypted;
            this.EncryptionData = encryptionData;
            this.CompressionData = compressionData;
            this.EncryptionData.Algorithm.Mode = this.EncryptionData.CipherMode;
            this.EncryptionData.Algorithm.Padding = this.EncryptionData.PaddingMode;
            this.PasswordData = passwordData;
        }
        /// <summary>
        /// Compresses the data via gzip (optimal)
        /// </summary>
        public void Compress()
        {
            this.Compress(CompressionAlgorithm.GZIP, CompressionLevel.Optimal);
        }
        /// <summary>
        /// Compresses the data
        /// </summary>
        /// <param name="compressionAlgorithm">The compression algorithm to use</param>
        /// <param name="compressionLevel">The compression level to use</param>
        /// <exception cref="ArgumentException">If data is already compressed or encrypted</exception>
        public void Compress(CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            if (!this.Compressed && !this.Encrypted)
            {
                this.Data = this._compression.Compress(this.Data, compressionAlgorithm, compressionLevel);
                this.CompressionData = new CompressionData(compressionAlgorithm, compressionLevel);
                this.Compressed = true;
            }
            else
                throw new ArgumentException("Data already compressed and/or encrypted");
        }
        /// <summary>
        /// Decompresses the data
        /// </summary>
        /// <exception cref="ArgumentException">If data is encrypted or not compressed</exception>
        public void Decompress()
        {
            if (this.Compressed && !this.Encrypted)
            {
                this.Data = this._compression.Decompress(this.Data, this.CompressionData.CompressionAlgorithm);
                this.CompressionData = null;
                this.Compressed = false;
                //this.TryCheckHash();
            }
            else
                throw new ArgumentException("Data already decompressed or encrypted");
        }

        /// <summary>
        /// Encrypts the data
        /// </summary>
        /// <param name="password">Password to use</param>
        /// <param name="algorithm">Encryption algorithm to use</param>
        /// <param name="hmacAlgorithm">Algorithm to use for HMAC, note that this will also determine the security of the encryption as it can be used to check for password validity</param>
        /// <exception cref="ArgumentException">If data is already encrypted</exception>
        public void Encrypt(Password password, SymmetricAlgorithm algorithm, HMAC hmacAlgorithm)
        {
            if (!this.Encrypted)
            {
                byte[] salt = Hashing.GetRandomBytes(password.PasswordHash.Length);
                byte[] iv = Hashing.GetRandomBytes(algorithm.BlockSize / 8);

                this.PasswordData = new PasswordData(password.Salt, password.HashAlgorithm, password.Iterations);
                this.Data = Encryption.Encrypt(this.Data, algorithm, password, iv, salt);

                byte[] hmac = hmacAlgorithm.ComputeHash(Hash.Create(this.Data, HashAlgorithm.Create(hmacAlgorithm.HashName)).HashData);
                hmacAlgorithm.Key = new byte[0];
                HMACData hmacData = new HMACData(hmac, hmacAlgorithm);
                this.EncryptionData = new EncryptionData(salt, iv, algorithm.Mode, algorithm.Padding, algorithm, hmacData);


                this.Encrypted = true;
            }
            else
                throw new ArgumentException("Data already encrypted");
        }
        /// <summary>
        /// Encrypts the data via AES (CBC, PKCS7, blocksize = 128), uses SHA256 for hmac
        /// </summary>
        /// <param name="password">The password to use</param>
        public void Encrypt(Password password)
        {
            SymmetricAlgorithm algorithm = Rijndael.Create();
            algorithm.Mode = CipherMode.CBC;
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.BlockSize = 128;
            this.Encrypt(password, algorithm, new HMACSHA256(password.PasswordHash));
        }
        /// <summary>
        /// Decrypts the data
        /// </summary>
        /// <param name="password">Password to use</param>
        /// <exception cref="ArgumentException">If the password is invalid</exception>
        public void Decrypt(string password)
        {
            if (CheckPassword(password))
            {
                Password p = Password.Create(password, this.PasswordData.Iterations, this.PasswordData.Salt, this.PasswordData.HashAlgorithm);

                this.Data = Encryption.Decrypt(this.Data, this.EncryptionData.Algorithm, p, this.EncryptionData.IV, this.EncryptionData.Salt);

                this.EncryptionData = null;
                this.PasswordData = null;
                this.Encrypted = false;
                //this.TryCheckHash();
            }
            else
                throw new ArgumentException("Password is not valid");
        }
        /*
        public bool CheckHash()
        {
            if (!(this.Compressed || this.Encrypted))
            {
                Hash hash = this.ComputeHash();
                if (this.Hash != hash)
                    this.IsCorrupted = true;
                this._needHashCheck = false;
                return this.IsCorrupted;
            }
            else
                throw new InvalidOperationException("Can't check hash of encrypted or compressed data");
        }*/
        /// <summary>
        /// Checks if the password is valid
        /// </summary>
        /// <param name="password">The password to use</param>
        /// <returns>True if the password is valid, false if not</returns>
        /// <exception cref="InvalidOperationException">If the data was not encrypted</exception>
        public bool CheckPassword(string password)
        {
            if (this.Encrypted)
            {
                Password p = Password.Create(password, this.PasswordData.Iterations, this.PasswordData.Salt, this.PasswordData.HashAlgorithm);

                this.EncryptionData.HMAC.HashAlgorithm.Key = p.PasswordHash;
                byte[] hmac = this.EncryptionData.HMAC.HashAlgorithm.ComputeHash(Hash.Create(this.Data, HashAlgorithm.Create(this.EncryptionData.HMAC.HashAlgorithm.HashName)).HashData);
                this.EncryptionData.HMAC.HashAlgorithm.Key = new byte[0];

                return hmac.SequenceEqual(this.EncryptionData.HMAC.Data);
            }
            else
                throw new InvalidOperationException("Data was not encrypted.");
        }
    
        private void Initialize(Type type, Encoding originalEncoding, byte[] data)
        {
            this.Type = type;
            this.OriginalEncoding = originalEncoding;
            this.Data = data;
        }
        /*
        private Hash ComputeHash()
        {
            if (!this.Compressed && !this.Encrypted)
            {
                return Hash.Create(this.Data, SHA256.Create());
            }
            else
                throw new ArgumentException("Data was compressed and/or encrypted and could not be hashed");
        }
        private void TryCheckHash()
        {
            if (!(this.Compressed || this.Encrypted))
            {
                Hash hash = this.ComputeHash();
                if (this.Hash != hash)
                    this.IsCorrupted = true;
                this._needHashCheck = false;
            }
        }*/
    }
}
