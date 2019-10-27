using System.Collections.Generic;

namespace CryptoCommon
{
    public interface IPaddingStandard
    {
        IEnumerable<IEnumerable<byte>> GetAlignedBlocks(IEnumerable<byte> lastBlock, int blockSize);
        IEnumerable<byte> GetData(IEnumerable<byte> lastBlock, int blockSize);
    }
}