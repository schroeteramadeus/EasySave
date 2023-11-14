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
        public string CompressToBase64(string text, Encoding fromEncoding, CompressionAlgorithm compressionAlgorithm, CompressionLevel compressionLevel)
        {
            byte[] data = fromEncoding.GetBytes(text);
            return Convert.ToBase64String(Compress(data, compressionAlgorithm, compressionLevel));
        }
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