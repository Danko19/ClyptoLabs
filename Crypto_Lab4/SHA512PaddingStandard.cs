using System;
using System.Collections.Generic;
using System.Linq;
using CryptoCommon;
using MoreLinq;

namespace Crypto_Lab4
{
	public class SHA512PaddingStandard : IPaddingStandard
	{
		public IEnumerable<IEnumerable<byte>> GetAlignedBlocks(IEnumerable<byte> lastBlock, int blockSize, long previousBlocksCount)
		{
			var lastBlockArray = lastBlock.ToArray();
			var lastBlockValueSize = blockSize - 8;
			var k = lastBlockValueSize - lastBlockArray.Length - 1;
			if (k < 0)
				k += blockSize;

			var valuableDataLength = previousBlocksCount * blockSize + lastBlockArray.Length;

			return lastBlockArray
				.Concat(new byte[] { 128 })
				.Concat(Enumerable.Repeat<byte>(0, k))
				.Concat(BitConverter.GetBytes((ulong)valuableDataLength * 8).Reverse())
				.Batch(blockSize);
		}

		public IEnumerable<byte> GetData(IEnumerable<byte> lastBlock, int blockSize)
		{
			throw new System.NotImplementedException();
		}
	}
}