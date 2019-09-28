using System;
using System.Collections.Generic;

namespace Crypto_Lab3
{
    public class SBlock
    {
        private readonly ushort[] s = new ushort[ushort.MaxValue];

        public SBlock(IReadOnlyList<byte> key)
        {
            Init(key);
        }

        private void Init(IReadOnlyList<byte> key)
        {
            for (var i = 0; i < ushort.MaxValue; i++)
                s[i] = (ushort)i;

            var j = 0;

            for (var i = 0; i < ushort.MaxValue; i++)
            {
                j = (j + s[i] + key[i % key.Count]) % ushort.MaxValue;
                Swap(i, j);
            }
        }

        public IEnumerable<byte> GetBytes()
        {
            var i = 0;
            var j = 0;

            while (true)
            {
                i = (i + 1) % ushort.MaxValue;
                j = (j + s[i]) % ushort.MaxValue;
                Swap(i, j);
                var t = (s[i] + s[j]) % ushort.MaxValue;
                var k = s[t];
                foreach (var b in BitConverter.GetBytes(k))
                    yield return b;
            }
        }

        private void Swap(int i, int j)
        {
            var buf = s[i];
            s[i] = s[j];
            s[j] = buf;
        }
    }
}