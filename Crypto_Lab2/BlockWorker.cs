using System.Collections.Generic;
using MoreLinq;

namespace Crypto_Lab2
{
    public class BlockWorker<TPaddingStandard>
        where TPaddingStandard : IPaddingStandard, new()
    {
        private readonly int blockSize;
        private readonly TPaddingStandard paddingStandard;

        public BlockWorker(int blockSize)
        {
            this.blockSize = blockSize;
            paddingStandard = new TPaddingStandard();
        }

        public IEnumerable<IEnumerable<byte>> GetBlocks(IEnumerable<byte> data)
        {
            IEnumerable<byte> previousBlock = null;

            foreach (var block in data.Batch(blockSize))
            {
                if (previousBlock != null)
                    yield return previousBlock;

                previousBlock = block;
            }

            if (previousBlock == null) yield break;

            foreach (var block in paddingStandard.GetAlignedBlocks(previousBlock, blockSize))
                yield return block;
        }

        public IEnumerable<byte> GetData(IEnumerable<IEnumerable<byte>> blocks)
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

            foreach (var b in paddingStandard.GetData(previousBlock, blockSize))
                yield return b;
        }
    }
}