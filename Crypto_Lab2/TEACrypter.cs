using System;
using System.Linq;

namespace Crypto_Lab2
{
    public class TEACrypter : ICrypter
    {
        protected const uint Delta = 2_654_435_769;

        protected readonly uint k0;
        protected readonly uint k1;
        protected readonly uint k2;
        protected readonly uint k3;

        public TEACrypter(byte[] key)
        {
            if (key.Length != 16)
                throw new ArgumentException("Length of key should be 128 bits");

            k0 = BitConverter.ToUInt32(key, 0);
            k1 = BitConverter.ToUInt32(key, 4);
            k2 = BitConverter.ToUInt32(key, 8);
            k3 = BitConverter.ToUInt32(key, 12);
        }

        public ICrypter Create(byte[] key)
        {
            return new TEACrypter(key);
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

        public int BlockSize => 8;
        public int KeySize => 16;
    }
}