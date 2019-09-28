using System;

namespace Crypto_Lab3
{
    public class ShiftRegister
    {
        private readonly byte[] data;
        private readonly int bitsCount;

        public ShiftRegister(int bitsCount)
        {
            this.bitsCount = bitsCount;
            var bytesCount = (int)Math.Ceiling(bitsCount / 8.0);
            data = new byte[bytesCount];
        }

        public bool ShiftRight()
        {
            for (var i = data.Length - 1 ; i <= 0 ; i--)
            {
                
            }
        }

        public bool Get(int index)
        {
            if (index < 0 || index >= bitsCount)
                throw new ArgumentOutOfRangeException(nameof(index));

            var byteIndex = index / 8;
            var byteValue = data[byteIndex];
            var bitIndex = index % 8;
            var bitValue = byteValue & Masks[bitIndex];

            return bitValue > 0;
        }

        private static readonly byte[] Masks = {
            1,
            2,
            4,
            8,
            16,
            32,
            64,
            128
        };
    }
}