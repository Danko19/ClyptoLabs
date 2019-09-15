using System;
using System.IO;
using System.Linq;
using System.Text;
using CryptoCommon;
using MoreLinq;

namespace Crypto_Lab1
{
    class Program
    {
        private static readonly Encoding encoding = Encoding.GetEncoding(1251);

        static void Main(string[] args)
        {
            var fileName = args.Single();
            var mode = ChooseMode();
            var fileContent = new LazyFileReader(fileName);

            if (mode == Mode.Encrypt)
            {
                var alphabet = GetAlphabet();
                var encryptedContent = new CaesarEncrypter(alphabet).Encrypt(fileContent);
                var fileWriter = new LazyFileWriter(GenerateFileName(fileName, "encrypted"));
                fileWriter.Write(encryptedContent);
            }

            if (mode == Mode.Decrypt)
            {
                var alphabet = GetAlphabet();
                var decrypt = new CaesarDecrypter(alphabet).Decrypt(fileContent);
                var fileWriter = new LazyFileWriter(GenerateFileName(fileName, "decrypted"));
                fileWriter.Write(decrypt);
            }

            if (mode == Mode.Analyze)
            {
                Console.WriteLine("Analyzing...");
                var byteFrequencies = new ByteFrequencyAnalyzer().Analyze(fileContent).ToArray();
                var index = 0;
                Console.WriteLine("The most common bytes:");
                foreach (var batch in byteFrequencies.Batch(10))
                {
                    foreach (var byteFrequency in batch)
                        Console.WriteLine($"{++index}. {byteFrequency}");

                    bool skip;
                    while (true)
                    {
                        Console.WriteLine("Continue? (y/n)");
                        var input = Console.ReadLine();
                        if (input == "Y" || input == "y")
                        {
                            skip = false;
                            break;
                        }

                        if (input == "N" || input == "n")
                        {
                            skip = true;
                            break;
                        }
                    }
                    if (skip) break;
                }

                var sb = new StringBuilder();
                sb.AppendLine("Байт,количество");
                foreach (var frequency in byteFrequencies)
                {
                    sb.AppendLine($"{frequency.Value},{frequency.Frequency}");
                }
                File.WriteAllText(GenerateFileName(fileName, "analyzed"), sb.ToString());
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
                Console.WriteLine("3) Analyze");
                var line = Console.ReadLine();
                if (int.TryParse(line, out var index))
                {
                    if (index == 1)
                        mode = Mode.Encrypt;
                    if (index == 2)
                        mode = Mode.Decrypt;
                    if (index == 3)
                        mode = Mode.Analyze;
                }

            } while (mode == Mode.Undefined);

            return mode;
        }

        private static CaesarAlphabet GetAlphabet()
        {
            var offset = 0;
            while (true)
            {
                Console.WriteLine("Enter offset 'k':");
                var line = Console.ReadLine();
                if (int.TryParse(line, out offset))
                    break;
            }

            Console.WriteLine("Enter key word:");
            var keyWord = Console.ReadLine();

            return new CaesarAlphabet(offset, keyWord);
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
            Decrypt,
            Analyze
        }
    }
}
