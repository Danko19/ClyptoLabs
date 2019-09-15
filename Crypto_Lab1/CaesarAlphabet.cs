using System;
using System.Linq;
using System.Text;

namespace Crypto_Lab1
{
    public class CaesarAlphabet
    {
        private readonly byte[] encryptRuler = new byte[256];
        private readonly byte[] decryptRuler = new byte[256];

        public CaesarAlphabet(int offset, string codeWord)
        {
            var preparedCodeWord = new string(codeWord.Distinct().ToArray());
            var codeWordBytes = Encoding.GetEncoding(1251).GetBytes(preparedCodeWord);
            var otherBytes = Enumerable
                .Range(0, 256)
                .Select(i => (byte)i)
                .Except(codeWordBytes)
                .ToArray();

            var alphabet = otherBytes.Skip(256 - offset)
                .Concat(codeWordBytes)
                .Concat(otherBytes.Take(256 - offset))
                .ToArray();

            if (alphabet.Length != 256)
                throw new ArgumentException();

            for (var i = 0; i < alphabet.Length; i++)
            {
                var newByte = alphabet[i];
                encryptRuler[i] = newByte;
                decryptRuler[newByte] = (byte)i;
            }
        }

        public byte EncryptByte(byte nextByte)
        {
            return encryptRuler[nextByte];
        }

        public byte DecryptByte(byte nextByte)
        {
            return decryptRuler[nextByte];
        }
    }
}