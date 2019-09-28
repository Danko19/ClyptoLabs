using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab3
{
    public class RC4lAgorithm
    {
        public IEnumerable<byte> Crypt(IEnumerable<byte> source , byte[] key)
        {
            return source.Zip(new SBlock(key).GetBytes(), (b, k) => (byte)(b ^ k));
        }
    }
}