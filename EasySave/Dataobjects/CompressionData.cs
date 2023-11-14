namespace EasySave.Dataobjects
{
    using EasySave.Crypto;
    using System.IO.Compression;

    public class CompressionData
    {
        //TODO
        //implement for jsonDataobject
        public CompressionAlgorithm CompressionAlgorithm { get; private set; }
        public CompressionLevel CompressionLevel { get; private set; }

        public CompressionData(CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            this.CompressionAlgorithm = compressionAlgorithm;
            this.CompressionLevel = compressionLevel;
        }
    }
}
