using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Crypto_Lab4
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var mode = ChooseMode();
            switch (mode)
            {
                case Mode.Keygen:
                    Keygen();
                    return;
                case Mode.Hash:
                    Hash();
                    return;
                case Mode.Sign:
                    Sign();
                    return;
                case Mode.CheckSign:
                    CheckSign();
                    return;
                case Mode.Encrypt:
                    Encrypt();
                    return;
                case Mode.Decrypt:
                    Decrypt();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Keygen()
        {
            PublicKey publicKey = null;
            PrivateKey privateKey = null;
            var task = Task.Run(() => (publicKey, privateKey) = RSAKeyGenerator.Generate());
            Console.Write("Keys generation in progress");
            do
            {
                if (task.IsCanceled && task.IsFaulted)
                {
                    Console.WriteLine();
                    Console.WriteLine("Some error occured, keys will not be generated");
                    return;
                }

                Console.Write('.');
                Thread.Sleep(1000);
            } while (!task.IsCompleted);

            publicKey.Save();
            privateKey.Save();
            Console.WriteLine();
            Console.WriteLine("Keys successfully generated");
        }

        private static void Hash()
        {
            var fileName = ChooseFile();

            var content = File.ReadAllBytes(fileName);
            var hash = SHA512.GetHash(content);
            var hex = hash.ToHexString();
            File.WriteAllText(GenerateFileName(fileName, "hash", ".hex"), hash.ToHexString());
            Console.WriteLine($"Hash is: {hex}");
        }

        private static void Sign()
        {
            var privateKey = PrivateKey.Load();
            var fileName = ChooseFile();
            var content = File.ReadAllBytes(fileName);
            var hash = SHA512.GetHash(content);
            var sign = RSA.Crypt(hash, privateKey);
            File.WriteAllBytes(GenerateFileName(fileName, "sign", ".bin"), sign);
            Console.WriteLine("File successfully signed");
        }

        private static void CheckSign()
        {
            var publicKey = PublicKey.Load();
            var fileName = ChooseFile(" (Document)");
            var document = File.ReadAllBytes(fileName);
            var hash = SHA512.GetHash(document);
            fileName = ChooseFile(" (Signature)");
            var signature = File.ReadAllBytes(fileName);
            var decryptedHash = RSA.Crypt(signature, publicKey);
            var isValid = decryptedHash.SequenceEqual(hash);
            Console.WriteLine($"Signature is {(isValid ? "" : "not ")}valid");
        }

        private static void Encrypt()
        {
            var publicKey = PublicKey.Load();
            var fileName = ChooseFile();
            var content = File.ReadAllBytes(fileName);
            var encrypted = RSA.Crypt(content, publicKey);
            File.WriteAllBytes(GenerateFileName(fileName, "encrypted"), encrypted);
            Console.WriteLine("File successfully encrypted");
        }

        private static void Decrypt()
        {
            var privateKey = PrivateKey.Load();
            var fileName = ChooseFile();
            var content = File.ReadAllBytes(fileName);
            var decrypted = RSA.Crypt(content, privateKey);
            File.WriteAllBytes(GenerateFileName(fileName, "decrypted"), decrypted);
            Console.WriteLine("File successfully decrypted");
        }


        private static Mode ChooseMode()
        {
            var mode = Mode.Undefined;
            do
            {
                Console.WriteLine("Choose mode:");
                Console.WriteLine("1) Keygen");
                Console.WriteLine("2) Hash");
                Console.WriteLine("3) Sign");
                Console.WriteLine("4) Check sign");
                Console.WriteLine("5) Encrypt");
                Console.WriteLine("6) Decrypt");
                var line = Console.ReadLine();
                if (!int.TryParse(line, out var index)) continue;
                if (index < 1 || index > 6)
                    continue;
                mode = (Mode) index;
            } while (mode == Mode.Undefined);

            return mode;
        }

        private static string ChooseFile(string comment = null)
        {
            string fileName = null;
            var first = true;
            do
            {
                if (!first)
                    Console.WriteLine($"File with name `{fileName}` was not found");

                Console.WriteLine($"Enter file name{comment}:");
                fileName = Console.ReadLine();
                first = false;
            } while (!File.Exists(fileName));

            return fileName;
        }

        private static string GenerateFileName(string fileName, string suffix, string resultExtension = null)
        {
            var extension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(extension))
                fileName = fileName.Replace($"{extension}", "");

            fileName = $"{fileName} ({suffix})";
            extension = resultExtension ?? extension;

            if (!string.IsNullOrWhiteSpace(extension))
                fileName = $"{fileName}{extension}";

            return fileName;
        }

        private enum Mode
        {
            Undefined,
            Keygen,
            Hash,
            Sign,
            CheckSign,
            Encrypt,
            Decrypt
        }
    }
}