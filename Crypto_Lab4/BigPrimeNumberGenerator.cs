using System;
using System.Numerics;
using System.Security.Cryptography;

namespace Crypto_Lab4
{
    public static class BigPrimeNumberGenerator
    {
        private static readonly RandomNumberGenerator randomBytesGenerator = new RNGCryptoServiceProvider();

        public static BigInteger GetRandomPrimeNumber(int bitsCount)
        {
            if (bitsCount % 8 != 0)
                throw new ArgumentException($"Parameter bitsCount={bitsCount} must be multiple of 8");

            var bytesCount = bitsCount / 8;
            return GetRandomPrimeNumberInternal(bytesCount);
        }

        private static BigInteger GetRandomPrimeNumberInternal(int bytesCount)
        {
            var rawData = new byte[bytesCount];

            while (true)
            {
                randomBytesGenerator.GetBytes(rawData);
                var primeCandidate = new BigInteger(rawData);

                if (primeCandidate.IsPrime())
                    return primeCandidate;
            }
        }
    }
}