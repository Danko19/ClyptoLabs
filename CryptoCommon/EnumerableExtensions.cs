using System;
using System.Collections.Generic;

namespace CryptoCommon
{
    public static class EnumerableExtensions
    {
        public static byte[] Xor(this byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                throw new ArgumentException(
                    $"Arrays have different sizes. First: {first.Length}, second {second.Length}");

            var result = new byte[first.Length];
            for (var i = 0; i < first.Length; i++)
            {
                result[i] = (byte)(first[i] ^ second[i]);
            }

            return result;
        }
    }
}