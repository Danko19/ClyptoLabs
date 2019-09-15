using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;

namespace CryptoCommon
{
    public class LazyFileWriter
    {
        private const int batchSize = 1024 * 1024;
        private readonly string fileName;

        public LazyFileWriter(string fileName)
        {
            this.fileName = fileName;
        }

        public void Write(IEnumerable<byte> content)
        {
            using (var fileStream = File.OpenWrite(fileName))
            using (var writer = new BinaryWriter(fileStream))
            {
                foreach (var batch in content.Batch(batchSize))
                    writer.Write(batch.ToArray<byte>());
            }
        }
    }
}