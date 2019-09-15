using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CryptoCommon
{
    public class LazyFileReader : IEnumerable<byte>
    {
        private const int batchSize = 1024 * 4;
        private readonly string fileName;

        public LazyFileReader(string fileName)
        {
            this.fileName = fileName;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            using (var fileStream = File.OpenRead(fileName))
            using (var reader = new BinaryReader(fileStream))
            {
                byte[] buffer = null;
                do
                {
                    buffer = reader.ReadBytes(batchSize);
                    foreach (var b in buffer)
                        yield return b;
                } while (buffer.Length == batchSize);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}