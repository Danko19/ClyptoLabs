using System.Numerics;

namespace Crypto_Lab4
{
    public interface IRSAKey
    {
        BigInteger Exponent { get; }
        BigInteger Modulus { get; }
    }

    public class PrivateKey : IRSAKey
    {
        public PrivateKey(BigInteger d, BigInteger n)
        {
            Exponent = d;
            Modulus = n;
        }

        public BigInteger Exponent { get; }
        public BigInteger Modulus { get; }
    }

    public class PublicKey : IRSAKey
    {
        public PublicKey(BigInteger e, BigInteger n)
        {
            Exponent = e;
            Modulus = n;
        }

        public BigInteger Exponent { get; }
        public BigInteger Modulus { get; }
    }
}