using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using CryptoCommon;

namespace Crypto_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = File.ReadAllBytes("key");
            var fileName = args.Single();
            var fileContent = new LazyFileReader(fileName);
            var prefix = fileContent.Take(2).ToArray();
            var cryptAsBmp = false;
            if (prefix[0] == 0x42 && prefix[1] == 0x4D)
                cryptAsBmp = IsCryptAsBmp();

            IEnumerable<byte> result = null;
            var rc4 = new RC4lAgorithm();
            if (cryptAsBmp)
            {
                var byteOffset = fileContent.Skip(10).Take(4).ToArray();
                var intOffset =BitConverter.ToInt32(byteOffset, 0);
                result = fileContent
                    .Take(intOffset)
                    .Concat(rc4.Crypt(
                        fileContent.Skip(intOffset), key));
            }
            else result = rc4.Crypt(fileContent, key);
            
            var fileWriter = new LazyFileWriter(GenerateFileName(fileName));
            fileWriter.Write(result);
        }

        private static bool IsCryptAsBmp()
        {
            while (true)
            {
                Console.WriteLine("File looks like BMP. Keep file structure unchanged? (y/n)");
                var input = Console.ReadLine();
                if (input == "Y" || input == "y")
                    return true;
                if (input == "N" || input == "n")
                    return false;
            }
        }

        private static string GenerateFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(extension))
                fileName = fileName.Replace($"{extension}", "");

            fileName = $"{fileName}_ crypted_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

            if (!string.IsNullOrWhiteSpace(extension))
                fileName = $"{fileName}{extension}";

            return fileName;
        }
    }
}
