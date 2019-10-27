using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab4
{
	public class SHA512
	{
		private readonly EratosthenesPrimeNumberProvider primeNumberProvider = new EratosthenesPrimeNumberProvider();

		public byte[] GetHash(IEnumerable<byte> content)
		{
			var h = InitializeVector();
			return null;
		}

		private static ulong[] InitializeVector()
		{
			var h = new ulong[8];
			SHA512Constannts.H.CopyTo(h, 0);
			return h;
		}
	}
}
