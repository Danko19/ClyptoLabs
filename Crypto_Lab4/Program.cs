using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Lab4
{
	class Program
	{
		static void Main(string[] args)
		{
			new SHA512().GetHash(new byte[] { 1, 3, 4 });
		}
	}
}
