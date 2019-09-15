using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab1
{
    public class CaesarDecrypter
    {
        private readonly CaesarAlphabet alphabet;

        public CaesarDecrypter(CaesarAlphabet alphabet)
        {
            this.alphabet = alphabet;
        }

        public IEnumerable<byte> Decrypt(IEnumerable<byte> content)
        {
            return content
                .Select(b => alphabet.DecryptByte(b));
        }
    }
}