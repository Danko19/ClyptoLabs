using System;

namespace Crypto_Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random(19);

            var block = new byte[8];
            var key = new byte[16];

            rnd.NextBytes(block);
            rnd.NextBytes(key);

            var encrypter = new TEAEncrypter(key);
            var decrypter = new TEADecrypter(key);
            var encrypt = encrypter.Encrypt(block);
            var decrypt = decrypter.Decrypt(encrypt);
        }
    }
}
