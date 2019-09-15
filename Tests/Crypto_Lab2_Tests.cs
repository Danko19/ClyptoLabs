using System;
using System.Linq;
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

            var crypter = new TEACrypter(key);
            var encrypt = crypter.Encrypt(block);
            var decrypt = crypter.Decrypt(encrypt);

            decrypt.Should().BeEquivalentTo(block, o => o.WithStrictOrdering());
        }

        [Test]
        public void ShouldEncryptAndDecryptManyBlocksByPCBCWithTEA()
        {
            var rnd = new Random(19);
            var data = new byte[42];
            var key = new byte[16];
            var initializingVector = new byte[8];
            rnd.NextBytes(data);
            rnd.NextBytes(key);
            rnd.NextBytes(initializingVector);

            var strategy = new PCBCCryptingStrategy<Iso10126PaddingStandard>(k => new TEACrypter(k));

            var encrypt = strategy.Encrypt(data, key, initializingVector).ToArray();
            var decrypt = strategy.Decrypt(encrypt, key, initializingVector).ToArray();

            decrypt.Should().BeEquivalentTo(data, o => o.WithStrictOrdering());
        }
    }
}
