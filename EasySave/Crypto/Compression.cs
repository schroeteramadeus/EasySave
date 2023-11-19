namespace EasySave.Crypto
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public class Compression
    {
        public int BufferLength { get; set; } = 4096;

        public void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[this.BufferLength];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        //public string Compress(string text, Encoding fromEncoding, Encoding toEncoding)
        //{
        //    byte[] data = fromEncoding.GetBytes(text);
        //    return toEncoding.GetString(Compress(data));
        //}
        /// <summary>
        /// Converts the text to a compressed base64 string
        /// </summary>
        /// <param name="text">The text to compress</param>
        /// <param name="fromEncoding">The encoding of the text</param>
        /// <param name="compressionAlgorithm">The compression algorithm to use</param>
        /// <param name="compressionLevel">The level of compression</param>
        /// <returns>The base64 string representation of the converted text</returns>
        public string CompressToBase64(string text, Encoding fromEncoding, CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            byte[] data = fromEncoding.GetBytes(text);
            return Convert.ToBase64String(Compress(data, compressionAlgorithm, compressionLevel));
        }
        /// <summary>
        /// Converts the data to compressed data
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <param name="compressionAlgorithm">The compression algorithm to use</param>
        /// <param name="compressionLevel">The level of compression</param>
        /// <returns>The compressed data</returns>
        public byte[] Compress(byte[] data, CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            using (MemoryStream outputStream = new MemoryStream())
            {
                switch (compressionAlgorithm)
                {
                    case CompressionAlgorithm.GZIP:
                        using (GZipStream compressStream = new GZipStream(outputStream, compressionLevel))
                        {
                            CopyTo(inputStream, compressStream);
                        }
                        break;
                    case CompressionAlgorithm.Deflate:
                        using (DeflateStream compressStream = new DeflateStream(outputStream, compressionLevel))
                        {
                            CopyTo(inputStream, compressStream);
                        }
                        break;
                    case CompressionAlgorithm.Brotli:
                        using (BrotliStream compressStream = new BrotliStream(outputStream, compressionLevel))
                        {
                            CopyTo(inputStream, compressStream);
                        }
                        break;
                }

                return outputStream.ToArray();
            }
        }
        /// <summary>
        /// Converts compressed data to non-compressed data
        /// </summary>
        /// <param name="data">The compressed data</param>
        /// <param name="compressionAlgorithm">The compression algorithm</param>
        /// <returns>The non-compressed data</returns>
        public byte[] Decompress(byte[] data, CompressionAlgorithm compressionAlgorithm)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            using (MemoryStream outputStream = new MemoryStream())
            {
                switch (compressionAlgorithm)
                {
                    case CompressionAlgorithm.GZIP:
                        using (GZipStream compressStream = new GZipStream(inputStream, CompressionMode.Decompress))
                        {
                            CopyTo(compressStream, outputStream);
                        }
                        break;
                    case CompressionAlgorithm.Deflate:
                        using (DeflateStream compressStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                        {
                            CopyTo(compressStream, outputStream);
                        }
                        break;
                    case CompressionAlgorithm.Brotli:
                        using (BrotliStream compressStream = new BrotliStream(inputStream, CompressionMode.Decompress))
                        {
                            CopyTo(compressStream, outputStream);
                        }
                        break;
                }

                return outputStream.ToArray();
            }
        }
        /// <summary>
        /// Converts compressed base64 text to non-compressed text
        /// </summary>
        /// <param name="text">The base64 string representation of the data</param>
        /// <param name="toEncoding">The encoding for the resulting string</param>
        /// <param name="compressionAlgorithm">The compression algorithm</param>
        /// <returns>The non-compressed string value of the data</returns>
        public string DecompressFromBase64(string text, Encoding toEncoding, CompressionAlgorithm compressionAlgorithm)
        {
            byte[] data = Convert.FromBase64String(text);
            return toEncoding.GetString(Decompress(data, compressionAlgorithm));
        }
        //public string Decompress(string text, Encoding fromEncoding, Encoding toEncoding)
        //{
        //    byte[] data = fromEncoding.GetBytes(text);
        //    return toEncoding.GetString(Decompress(data));
        //}
    }
}