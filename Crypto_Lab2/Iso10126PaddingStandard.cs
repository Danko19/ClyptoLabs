using System;
using System.Collections.Generic;
using System.Linq;

namespace Crypto_Lab2
{
    public class Iso10126PaddingStandard : IPaddingStandard
    {
        public IEnumerable<IEnumerable<byte>> GetAlignedBlocks(IEnumerable<byte> lastBlock, int blockSize)
        {
            var random = new Random();
            var lastBlockArray = lastBlock.ToArray();

            if (lastBlockArray.Length == blockSize)
            {
                yield return lastBlockArray;

                var additionBlock = new byte[blockSize];
                random.NextBytes(additionBlock);
                additionBlock[blockSize - 1] = (byte)blockSize;
                yield return additionBlock;
                yield break;
            }

            var newLastBlock = new byte[blockSize];
            random.NextBytes(newLastBlock);
            lastBlockArray.CopyTo(newLastBlock, 0);
            newLastBlock[blockSize - 1] = (byte)(blockSize - lastBlockArray.Length);

            yield return newLastBlock;
        }

        public IEnumerable<byte> GetData(IEnumerable<byte> lastBlock, int blockSize)
        {
            var lastBlockArray = lastBlock.ToArray();
            var lastByte = lastBlockArray[blockSize - 1];

            if (lastByte == blockSize)
                yield break;

            var lastBlockData = lastBlockArray.Take(blockSize - lastByte);

            foreach (var b in lastBlockData)
                yield return b;
        }
    }
}