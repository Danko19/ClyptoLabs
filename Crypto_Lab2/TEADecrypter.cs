using System;
using System.Linq;

namespace Crypto_Lab2
{
    public class TEADecrypter : TEACrypterBase
    {
        public TEADecrypter(byte[] key) : base(key)
        {
        }

        public byte[] Decrypt(byte[] source)
        {
            if (source.Length != 8)
                throw new ArgumentException("Length of block should be 64 bits");

            var left = BitConverter.ToUInt32(source, 0);
            var right = BitConverter.ToUInt32(source, 4);
            var sum = unchecked(Delta * 32);

            for (var i = 0; i < 32; i++)
            {
                (left, right) = DoDecryptRound(left, right, sum);
                sum = unchecked(sum - Delta);
            }

            return BitConverter.GetBytes(left)
                .Concat(BitConverter.GetBytes(right))
                .ToArray();
        }

        private (uint left, uint right) DoDecryptRound(uint left, uint right, uint sum)
        {
            (left, right) = (right - (((left << 4) + k2) ^ (left + sum) ^ ((left >> 5) + k3)), left);
            (left, right) = (right - (((left << 4) + k0) ^ (left + sum) ^ ((left >> 5) + k1)), left);
            return (left, right);
        }
    }
}