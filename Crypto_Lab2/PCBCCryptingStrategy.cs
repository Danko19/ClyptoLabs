using System;
using System.Collections.Generic;
using System.Linq;
using CryptoCommon;
using MoreLinq;

namespace Crypto_Lab2
{
    public class PCBCCryptingStrategy<TPaddingStandart> where TPaddingStandart : IPaddingStandard, new()
    {
        private readonly Func<byte[], ICrypter> createCrypterByKey;

        public PCBCCryptingStrategy(Func<byte[], ICrypter> createCrypterByKey)
        {
            this.createCrypterByKey = createCrypterByKey;
        }

        public IEnumerable<byte> Encrypt(IEnumerable<byte> source, byte[] key, byte[] initializingVector)
        {
            var crypter = CreateCrypterWithChecks(key, initializingVector);

            var blockWorker = new BlockWorker<TPaddingStandart>(crypter.BlockSize);

            foreach (var block in blockWorker.GetBlocks(source))
            {
                var blockArray = block.ToArray();

                var encryptInput = blockArray.Xor(initializingVector);
                var encryptOutput = crypter.Encrypt(encryptInput);

                foreach (var b in encryptOutput)
                    yield return b;

                initializingVector = blockArray.Xor(encryptOutput);
            }
        }

        public IEnumerable<byte> Decrypt(IEnumerable<byte> source, byte[] key, byte[] initializingVector)
        {
            var crypter = CreateCrypterWithChecks(key, initializingVector);

            var blocks = DecryptInternal(crypter, source, initializingVector);

            var blockWorker = new BlockWorker<TPaddingStandart>(crypter.BlockSize);
            return blockWorker.GetData(blocks);
        }

        private static IEnumerable<IEnumerable<byte>> DecryptInternal(ICrypter crypter, IEnumerable<byte> source, byte[] initializingVector)
        {
            foreach (var block in source.Batch(crypter.BlockSize))
            {
                var blockArray = block.ToArray();
                var decryptOutput = crypter.Decrypt(blockArray);
                var decrypt = decryptOutput.Xor(initializingVector);
                yield return decrypt;
                initializingVector = blockArray.Xor(decrypt);
            }
        }

        private ICrypter CreateCrypterWithChecks(byte[] key, byte[] initializingVector)
        {
            var crypter = createCrypterByKey(key);

            if (crypter.KeySize != key.Length)
                throw new ArgumentException($"Key should have length of {crypter.KeySize} bytes");
            if (crypter.BlockSize != initializingVector.Length)
                throw new ArgumentException($"InitializingVector should have length of {crypter.BlockSize} bytes");

            return crypter;
        }
    }
}