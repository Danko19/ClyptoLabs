using System.Numerics;

namespace Crypto_Lab4
{
    public static class RSA
    {
        public static byte[] Crypt(byte[] content, IRSAKey key)
        {
            return BigInteger.ModPow(new BigInteger(content), key.Exponent, key.Modulus).ToByteArray();
        }
    }
}