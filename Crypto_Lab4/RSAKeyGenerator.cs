using System.Numerics;

namespace Crypto_Lab4
{
    public static class RSAKeyGenerator
    {
        private const int Exponent = 65537;
        private const int KeyBitDepth = 2048;

        public static (PublicKey publicKey, PrivateKey privateKey) Generate()
        {
            var p = BigPrimeNumberGenerator.GetRandomPrimeNumber(KeyBitDepth / 2);
            var q = BigPrimeNumberGenerator.GetRandomPrimeNumber(KeyBitDepth / 2);
            var n = BigInteger.Multiply(p, q);
            var fi_n = BigInteger.Multiply(p - 1, q - 1);
            var e = new BigInteger(Exponent);
            var d = ExtendedEuclideanAlgorithm.GetGcd(fi_n, e).y;

            if (d < 0)
                d += fi_n;

            var publicKey = new PublicKey(e, n);
            var privateKey = new PrivateKey(d, n);
            return (publicKey, privateKey);
        }
    }
}