using System;
using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab1
{
    public class ByteFrequencyAnalyzer
    {
        public IEnumerable<ByteFrequency> Analyze(IEnumerable<byte> source)
        {
            var counter = new int[256];
            foreach (var b in source)
                counter[b]++;

            return counter
                .Select((c, b) => new ByteFrequency
                {
                    Frequency = c,
                    Value = (byte) b
                })
                .OrderByDescending(x => x.Frequency);
        }
    }
}