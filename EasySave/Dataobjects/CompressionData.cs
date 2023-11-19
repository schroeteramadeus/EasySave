namespace EasySave.Dataobjects
{
    using EasySave.Crypto;
    using System.IO.Compression;

    /// <summary>
    /// Data class for compression related meta data
    /// </summary>
    public class CompressionData
    {
        /// <summary>
        /// The compression algorithm used
        /// </summary>
        public CompressionAlgorithm CompressionAlgorithm { get; private set; }
        /// <summary>
        /// The compression level used
        /// </summary>
        public CompressionLevel CompressionLevel { get; private set; }

        public CompressionData(CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            this.CompressionAlgorithm = compressionAlgorithm;
            this.CompressionLevel = compressionLevel;
        }
    }
}
