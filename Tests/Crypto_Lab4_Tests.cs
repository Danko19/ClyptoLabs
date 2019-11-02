using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Crypto_Lab4;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class Crypto_Lab4_Tests
    {
        private readonly Lazy<HashSet<int>> primeNumbersFrom2To100000 =
            new Lazy<HashSet<int>>(() => new EratosthenesPrimeNumberProvider().GetPrimeNumbersFrom2To(100000).ToHashSet());

        [Test]
        public void BigIntegerExtensionTest()
        {
            foreach (var number in Enumerable.Range(2, 99999))
            {
                var actual = new BigInteger(number).IsPrime();
                var expected = primeNumbersFrom2To100000.Value.Contains(number);
                actual.Should().Be(expected, $"Number {number} is prime actual: {actual}, expected: {expected}");
            }
        }

        [Test]
        public void BigPrimeNumberGeneratorTest()
        {
            for (var i = 0; i < 100; i++)
            {
                var randomPrimeNumber = BigPrimeNumberGenerator.GetRandomPrimeNumber(16);
                primeNumbersFrom2To100000.Value.Contains((int) randomPrimeNumber).Should()
                    .BeTrue($"{(int) randomPrimeNumber} is unknown prime number");
            }
        }

        [Test]
        public void SHA512HashTest()
        {
            var data = new byte[123456];
            new Random().NextBytes(data);
            var hash1 = SHA512.GetHash(data);
            var hash2 = System.Security.Cryptography.SHA512.Create().ComputeHash(data);
            hash1.Should().BeEquivalentTo(hash2, options => options.WithStrictOrdering());
        }

        [Test]
        public void SHA512PaddingStandardShouldReturnSingleBlock()
        {
            var data = new byte[] {1, 2, 3};
            var paddingStandard = new SHA512PaddingStandard();
            var result = paddingStandard.GetAlignedBlocks(data, 16, 1).ToArray();

            result.Length.Should().Be(1);
            var block = result.Single();
            block.Should().BeEquivalentTo(new byte[] {1, 2, 3, 128}
                    .Concat(Enumerable.Repeat((byte) 0, 11))
                    .Concat(new byte[] {19 * 8})
                    .ToArray(),
                options => options.WithStrictOrdering());
        }

        [Test]
        public void SHA512PaddingStandardShouldReturnTwoBlocks()
        {
            var data = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};
            var paddingStandard = new SHA512PaddingStandard();

            var result = paddingStandard
                .GetAlignedBlocks(data, 16, 0)
                .ToArray();

            result.Length.Should().Be(2);
            var firstBlock = result.First();
            var secondBlock = result.Last();
            firstBlock.Should().BeEquivalentTo(new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 128}
                    .Concat(Enumerable.Repeat((byte) 0, 7))
                    .ToArray(),
                options => options.WithStrictOrdering());
            secondBlock.Should().BeEquivalentTo(Enumerable.Repeat((byte) 0, 15)
                    .Concat(new byte[] {8 * 8})
                    .ToArray(),
                options => options.WithStrictOrdering());
        }

        [Test]
        [TestCase(1234, 54, 2, -7, 160)]
        [TestCase(54, 1234, 2, 160, -7)]
        [TestCase(1234, 1234, 1234, 0, 1)]
        [TestCase(9167368, 3, 1, 1, -3055789)]
        public void ExtendedEuclideanAlgorithmTest(int a, int b, int expectedGcd, int expectedX, int expectedY)
        {
            var (gcd, x, y) = ExtendedEuclideanAlgorithm.GetGcd(new BigInteger(a), new BigInteger(b));
            gcd.Should().Be(expectedGcd);
            x.Should().Be(expectedX);
            y.Should().Be(expectedY);
        }

        [Test]
        public void RSASimpleTest()
        {
            var data = new BigInteger(123456).ToByteArray();
            var (publicKey, privateKey) = RSAKeyGenerator.Generate();
            var sign = RSA.Crypt(data, privateKey);
            var decryptedData = RSA.Crypt(sign, publicKey);
            data.Should().BeEquivalentTo(decryptedData, options => options.WithStrictOrdering());
        }

        [Test]
        public void RSAFunctionalTest()
        {
            var data = new byte[123456];
            new Random().NextBytes(data);
            var hash = SHA512.GetHash(data);
            var (publicKey, privateKey) = RSAKeyGenerator.Generate();
            var sign = RSA.Crypt(hash, privateKey);
            var decryptedHash = RSA.Crypt(sign, publicKey);
            hash.Should().BeEquivalentTo(decryptedHash, options => options.WithStrictOrdering());
        }
    }
}