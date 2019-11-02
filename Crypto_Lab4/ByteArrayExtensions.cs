using System.Collections.Generic;
using System.Text;

namespace Crypto_Lab4
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this IEnumerable<byte> source)
        {
            var hex = new StringBuilder();
            foreach (var b in source)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}