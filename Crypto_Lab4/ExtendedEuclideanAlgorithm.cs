using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Crypto_Lab4
{
    public static class ExtendedEuclideanAlgorithm
    {
        /// <summary>
        ///     Returns greatest common divisor of numbers a and b. Auxiliary values x and y is such that a*x+b*y=gcd
        /// </summary>
        public static (BigInteger gcd, BigInteger x, BigInteger y) GetGcd(BigInteger a, BigInteger b)
        {
            if (a >= b) return GetGcdInternal(a, b);
            var (gcd, x, y) = GetGcdInternal(b, a);
            return (gcd, y, x);
        }

        private static (BigInteger gcd, BigInteger x, BigInteger y) GetGcdInternal(BigInteger a, BigInteger b)
        {
            var table = new List<EuclideanTableItem>
            {
                new EuclideanTableItem
                {
                    R = b,
                    X = BigInteger.Zero,
                    Y = BigInteger.One
                },
                new EuclideanTableItem
                {
                    R = a,
                    X = BigInteger.One,
                    Y = BigInteger.Zero
                }
            };

            while (!table.First().R.IsZero)
            {
                var n2 = table[1];
                var n1 = table[0];
                var q = BigInteger.DivRem(n2.R, n1.R, out var r);

                var nextTableItem = new EuclideanTableItem
                {
                    Q = q,
                    R = r,
                    X = n2.X - BigInteger.Multiply(n1.X, q),
                    Y = n2.Y - BigInteger.Multiply(n1.Y, q)
                };

                table.RemoveAt(1);
                table.Insert(0, nextTableItem);
            }

            var item = table[1];
            return (item.R, item.X, item.Y);
        }

        private class EuclideanTableItem
        {
            public BigInteger R { get; set; }
            public BigInteger Q { get; set; }
            public BigInteger X { get; set; }
            public BigInteger Y { get; set; }
        }
    }
}