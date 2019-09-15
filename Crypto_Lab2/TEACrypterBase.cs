using System;

namespace Crypto_Lab2
{
    public abstract class TEACrypterBase
    {
        protected const uint Delta = 2_654_435_769;

        protected readonly uint k0;
        protected readonly uint k1;
        protected readonly uint k2;
        protected readonly uint k3;

        protected TEACrypterBase(byte[] key)
        {
            if (key.Length != 16)
                throw new ArgumentException("Length of key should be 128 bits");

            k0 = BitConverter.ToUInt32(key, 0);
            k1 = BitConverter.ToUInt32(key, 4);
            k2 = BitConverter.ToUInt32(key, 8);
            k3 = BitConverter.ToUInt32(key, 12);
        }
    }
}