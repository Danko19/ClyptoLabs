using System;
using System.IO;
using System.Linq;
using CryptoCommon;

namespace Crypto_Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = args.Single();
            var mode = ChooseMode();
            var fileContent = new LazyFileReader(fileName);
            var strategy = new PCBCCryptingStrategy<Iso10126PaddingStandard>(k => new TEACrypter(k));
            var key = File.ReadAllBytes("key");
            var initializingVector = File.ReadAllBytes("initializingVector");

            if (mode == Mode.Encrypt)
            {
                var encryptedContent = strategy.Encrypt(fileContent, key, initializingVector);
                var fileWriter = new LazyFileWriter(GenerateFileName(fileName, "encrypted"));
                fileWriter.Write(encryptedContent);
            }

            if (mode == Mode.Decrypt)
            {
                var decrypt = strategy.Decrypt(fileContent, key, initializingVector);
                var fileWriter = new LazyFileWriter(GenerateFileName(fileName, "decrypted"));
                fileWriter.Write(decrypt);
            }
        }

        private static Mode ChooseMode()
        {
            Mode mode = Mode.Undefined;
            do
            {
                Console.WriteLine("Choose mode:");
                Console.WriteLine("1) Encrypt");
                Console.WriteLine("2) Decrypt");
                var line = Console.ReadLine();
                if (int.TryParse(line, out var index))
                {
                    if (index == 1)
                        mode = Mode.Encrypt;
                    if (index == 2)
                        mode = Mode.Decrypt;
                }

            } while (mode == Mode.Undefined);

            return mode;
        }

        private static string GenerateFileName(string fileName, string suffix)
        {
            var extension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(extension))
                fileName = fileName.Replace($"{extension}", "");

            fileName = $"{fileName} ({suffix})";

            if (!string.IsNullOrWhiteSpace(extension))
                fileName = $"{fileName}{extension}";

            return fileName;
        }

        enum Mode
        {
            Undefined,
            Encrypt,
            Decrypt
        }
    }
}
