using System;
using System.Collections.Generic;
using System.Linq;
using CryptoCommon;
using static Crypto_Lab4.SHA512Constannts;

namespace Crypto_Lab4
{
	public static class SHA512
	{
		public static byte[] GetHash(IEnumerable<byte> content)
		{
			var hVector = InitializeVector();
			var blockWorker = new BlockWorker(128);

			foreach (var block in blockWorker.GetBlocks(content, new SHA512PaddingStandard()))
			{
				var blockArray = block.ToArray();
				var subBlocks = new ulong[80];
				for (var i = 0; i < 16; i++)
					subBlocks[i] = ExtendedBitConverter.ToUInt64(blockArray, i * 8);
				for (var i = 16; i < 80; i++)
				{
					var e0 = subBlocks[i - 15];
					var e1 = subBlocks[i - 2];
					var s0 = e0.Rotr(1).Xor(e0.Rotr(8)).Xor(e0 >> 7);
					var s1 = e1.Rotr(19).Xor(e1.Rotr(61)).Xor(e1 >> 6);
					subBlocks[i] = unchecked(subBlocks[i - 16] + s0 + subBlocks[i - 7] + s1);
				}

				var a = hVector[0];
				var b = hVector[1];
				var c = hVector[2];
				var d = hVector[3];
				var e = hVector[4];
				var f = hVector[5];
				var g = hVector[6];
				var h = hVector[7];

				for (var i = 0; i < 80; i++)
				{
					var sum0 = a.Rotr(28).Xor(a.Rotr(34)).Xor(a.Rotr(39));
					var ma = (a & b).Xor(a & c).Xor(b & c);
					var t2 = unchecked(sum0 + ma);
					var sum1 = e.Rotr(14).Xor(e.Rotr(18)).Xor(e.Rotr(41));
					var ch = (e & f).Xor(~e & g);
					var t1 = unchecked(h + sum1 + ch + K[i] + subBlocks[i]);

					h = g;
					g = f;
					f = e;
					e = unchecked(d + t1);
					d = c;
					c = b;
					b = a;
					a = unchecked(t1 + t2);
				}

				hVector[0] = unchecked(hVector[0] + a);
				hVector[1] = unchecked(hVector[1] + b);
				hVector[2] = unchecked(hVector[2] + c);
				hVector[3] = unchecked(hVector[3] + d);
				hVector[4] = unchecked(hVector[4] + e);
				hVector[5] = unchecked(hVector[5] + f);
				hVector[6] = unchecked(hVector[6] + g);
				hVector[7] = unchecked(hVector[7] + h);
			}

			return hVector
				.SelectMany(x => BitConverter.GetBytes(x).Reverse())
				.ToArray();
		}

		private static ulong[] InitializeVector()
		{
			var h = new ulong[8];
			H.CopyTo(h, 0);
			return h;
		}
	}
}
