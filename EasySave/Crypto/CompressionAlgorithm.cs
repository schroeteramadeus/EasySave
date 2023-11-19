using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.Crypto
{
    /// <summary>
    /// Available (de)compression algorithms
    /// </summary>
    public enum CompressionAlgorithm
    {
        GZIP,
        Deflate,
        Brotli
    }
}
