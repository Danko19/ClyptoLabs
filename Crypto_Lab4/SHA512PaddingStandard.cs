using System.Collections.Generic;
using System.Linq;
using CryptoCommon;

namespace Crypto_Lab4
{
	public class SHA512PaddingStandard : IPaddingStandard
	{
		public IEnumerable<IEnumerable<byte>> GetAlignedBlocks(IEnumerable<byte> lastBlock, int blockSize)
		{
			var lastBlockArray = lastBlock.ToArray();
		}

		public IEnumerable<byte> GetData(IEnumerable<byte> lastBlock, int blockSize)
		{
			throw new System.NotImplementedException();
		}
	}
}