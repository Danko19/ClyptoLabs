using System;
using Crypto_Lab2;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Crypto_Lab2_Tests
    {
        [Test]
        public void ShouldEncryptAndDecryptFullBlockByTEA()
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

            decrypt.Should().BeEquivalentTo(block, o => o.WithStrictOrdering());
        }
    }
}
