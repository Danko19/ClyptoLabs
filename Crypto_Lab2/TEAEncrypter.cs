using System;
using System.Linq;

namespace Crypto_Lab2
{
    public class TEAEncrypter : TEACrypterBase
    {
        public TEAEncrypter(byte[] key) : base(key)
        {
        }
        
        public byte[] Encrypt(byte[] source)
        {
            if (source.Length != 8)
                throw new ArgumentException("Length of block should be 64 bits");

            var left = BitConverter.ToUInt32(source, 0);
            var right = BitConverter.ToUInt32(source, 4);
            uint sum = 0;

            for (var i = 0; i < 32; i++)
            {
                sum = unchecked(sum + Delta);
                (left, right) = DoEncryptRound(left, right, sum);
            }

            return BitConverter.GetBytes(left)
                .Concat(BitConverter.GetBytes(right))
                .ToArray();
        }

        private (uint left, uint right) DoEncryptRound(uint left, uint right, uint sum)
        {
            unchecked
            {
                (left, right) = (right, left + (((right << 4) + k0) ^ (right + sum) ^ ((right >> 5) + k1)));
                (left, right) = (right, left + (((right << 4) + k2) ^ (right + sum) ^ ((right >> 5) + k3)));
                return (left, right);
            }
        }
    }
}