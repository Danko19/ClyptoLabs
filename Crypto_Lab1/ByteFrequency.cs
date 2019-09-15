using System;
using System.Text;

namespace Crypto_Lab1
{
    public class ByteFrequency
    {
        public byte Value { get; set; }
        public int Frequency { get; set; }

        public override string ToString()
        {
            return $"{Value} [{encoding.GetString(new[] {Value})}] ({Frequency} times)";
        }

        private static readonly Encoding encoding = Encoding.GetEncoding(1251);
    }
}