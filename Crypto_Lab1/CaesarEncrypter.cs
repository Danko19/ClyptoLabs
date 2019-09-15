using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab1
{
    public class CaesarEncrypter
    {
        private readonly CaesarAlphabet alphabet;

        public CaesarEncrypter(CaesarAlphabet alphabet)
        {
            this.alphabet = alphabet;
        }

        public IEnumerable<byte> Encrypt(IEnumerable<byte> content)
        {
            return content
                .Select(b => alphabet.EncryptByte(b));
        }
    }
}