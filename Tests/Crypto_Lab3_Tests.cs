using System;
using System.Linq;
using System.Text;
using Crypto_Lab3;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class Crypto_Lab3_Tests
    {
        private readonly Random random = new Random();

        [Test]
        public void RC4ShouldEncryptAndDecrypt()
        {
            var data = new byte[1333];
            var key = new byte[64];
            random.NextBytes(data);
            random.NextBytes(key);
            var rc4LAgorithm = new RC4lAgorithm();

            var encrypt = rc4LAgorithm.Crypt(data, key).ToArray();
            var encrypt2 = rc4LAgorithm.Crypt(data, key).ToArray();
            encrypt.Should().NotBeEquivalentTo(data);
            encrypt.Should().BeEquivalentTo(encrypt2, options => options.WithStrictOrdering());
            var decrypt = rc4LAgorithm.Crypt(encrypt, key).ToArray();
            decrypt.Should().BeEquivalentTo(data, options => options.WithStrictOrdering());
        }
    }
}