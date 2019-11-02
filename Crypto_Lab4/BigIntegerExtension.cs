using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Crypto_Lab4
{
    public static class BigIntegerExtension
    {
        private static readonly RandomNumberGenerator RandomNumberGenerator = new RNGCryptoServiceProvider();

        private static readonly HashSet<BigInteger> FirstPrimeNumbers = new HashSet<BigInteger>(
            new EratosthenesPrimeNumberProvider()
                .GetPrimeNumbersFrom2To(2000)
                .Select(pn => new BigInteger(pn)));

        public static bool IsPrime(this BigInteger number)
        {
            if (FirstPrimeNumbers.Contains(number))
                return true;

            if (number.IsEven || number.Sign != 1)
                return false;

            if (FirstPrimeNumbers.Any(pn => BigInteger.Remainder(number, pn).IsZero))
                return false;

            var (s, d) = Decompose(number);
            var numberLength = number.ToByteArray().Length;
            var millerRabinTestRounds = 5;
            var rawPrimeWitness = new byte[numberLength];
            for(var round = 0; round < millerRabinTestRounds; round++)
            {
                BigInteger primeWitness;
                do
                {
                    RandomNumberGenerator.GetBytes(rawPrimeWitness);
                    primeWitness = new BigInteger(rawPrimeWitness);
                }
                while (primeWitness < 2 || primeWitness > number - 2);

                var x = BigInteger.ModPow(primeWitness, d, number);

                if (x == 1 || x == number - 1)
                    continue;

                for (var i = 1; i < s; i++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1)
                        return false;
                    if (x == number - 1)
                        break;
                }

                if (x != number - 1)
                    return false;
            }

            return true;
        }

        private static (BigInteger s, BigInteger d) Decompose(BigInteger primeCandidate)
        {
            var d = primeCandidate - 1;
            var sInt = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                sInt += 1;
            }

            var s = new BigInteger(sInt);
            return (s, d);
        }
    }
}