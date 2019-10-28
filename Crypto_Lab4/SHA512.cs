using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCommon;
using MoreLinq;

namespace Crypto_Lab4
{
	public class SHA512
	{
		private readonly EratosthenesPrimeNumberProvider primeNumberProvider = new EratosthenesPrimeNumberProvider();

		public byte[] GetHash(IEnumerable<byte> content)
		{
			var hs = InitializeVector();
			var blockWorker = new BlockWorker(128);

			foreach (var block in blockWorker.GetBlocks(content, new SHA512PaddingStandard()))
			{
				var blockArray = block.ToArray();
				var subBlocks = new ulong[64];
				for (var i = 0; i < 16; i++)
					subBlocks[i] = BitConverter.ToUInt64(blockArray, i * 8);
				for (var i = 16; i < 64; i++)
				{
					var e0 = subBlocks[i - 15];
					var e1 = subBlocks[i - 2];
					var s0 = e0.Rotr(7).Xor(e0.Rotr(18)).Xor(e0 >> 3);
					var s1 = e1.Rotr(17).Xor(e1.Rotr(19)).Xor(e1 >> 10);
					unchecked
					{
						subBlocks[i] = subBlocks[i - 16] + s0 + subBlocks[i - 7] + s1;
					}
				}

				var a = hs[0];
				var b = hs[1];
				var c = hs[2];
				var d = hs[3];
				var e = hs[4];
				var f = hs[5];
				var g = hs[6];
				var h = hs[7];

				for (var i = 0; i < 80; i++)
				{

				}
			}

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
