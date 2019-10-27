using System.Collections.Generic;
using MoreLinq;

namespace CryptoCommon
{
	public class BlockWorker
	{
		private readonly int blockSize;

		public BlockWorker(int blockSize)
		{
			this.blockSize = blockSize;
		}

		public IEnumerable<IEnumerable<byte>> GetBlocks(IEnumerable<byte> data, IPaddingStandard lastBlockPadding)
		{
			IEnumerable<byte> previousBlock = null;
			var previousBlokcsCount = 0;

			foreach (var block in data.Batch(blockSize))
			{
				if (previousBlock != null)
				{
					previousBlokcsCount++;
					yield return previousBlock;
				}

				previousBlock = block;
			}

			if (previousBlock == null) yield break;

			foreach (var block in lastBlockPadding.GetAlignedBlocks(previousBlock, blockSize, previousBlokcsCount))
				yield return block;
		}

		public IEnumerable<byte> GetData(IEnumerable<IEnumerable<byte>> blocks, IPaddingStandard lastBlockTransformer)
		{
			IEnumerable<byte> previousBlock = null;

			foreach (var block in blocks)
			{
				if (previousBlock != null)
				{
					foreach (var b in previousBlock)
						yield return b;
				}

				previousBlock = block;
			}

			if (previousBlock == null) yield break;

			foreach (var b in lastBlockTransformer.GetData(previousBlock, blockSize))
				yield return b;
		}
	}
}