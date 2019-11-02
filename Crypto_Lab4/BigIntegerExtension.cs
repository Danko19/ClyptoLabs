using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MoreLinq.Extensions;

namespace Crypto_Lab4
{
    public static class BigIntegerExtension
    {
        private const int MillerRabinTestRounds = 5;

        private static readonly Random random = new Random();

        private static readonly HashSet<BigInteger> firstPrimeNumbers = new HashSet<BigInteger>(
            new EratosthenesPrimeNumberProvider()
                .GetPrimeNumbersFrom2To(2000)
                .Select(pn => new BigInteger(pn)));

        public static bool IsPrime(this BigInteger number)
        {
            if (firstPrimeNumbers.Contains(number))
                return true;

            if (number.IsEven || number.Sign != 1)
                return false;

            if (firstPrimeNumbers.Any(pn => BigInteger.Remainder(number, pn).IsZero))
                return false;

            var primeWitnesses = Enumerable
                .Range(2, 10000)
                .Shuffle(random)
                .Take(MillerRabinTestRounds)
                .Select(pw => new BigInteger(pw))
                .ToArray();

            var (s, d) = Decompose(number);
            foreach (var primeWitness in primeWitnesses)
            {
                var remainder = BigInteger.ModPow(primeWitness, d, number);
                if (remainder.IsOne || remainder == number - 1)
                    continue;

                for (var i = 1; i < (int) s; i++)
                {
                    remainder = BigInteger.ModPow(remainder, 2, number);
                    if (remainder.IsOne)
                        return false;
                    if (remainder == number - 1)
                        break;
                }
            }

            return true;
        }

        private static (BigInteger s, BigInteger d) Decompose(BigInteger primeCandidate)
        {
            var primeCandidateMinusOne = BigInteger.Subtract(primeCandidate, BigInteger.One);
            var sInt = -1;
            var two = new BigInteger(2);
            var divided = primeCandidateMinusOne;
            BigInteger remainder;

            do
            {
                sInt++;
                divided = BigInteger.DivRem(divided, two, out remainder);
            } while (remainder.IsZero);

            var s = new BigInteger(sInt);
            var divisor = BigInteger.Pow(two, sInt);
            var d = BigInteger.Divide(primeCandidateMinusOne, divisor);
            return (s, d);
        }
    }
}