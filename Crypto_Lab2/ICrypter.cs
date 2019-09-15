namespace Crypto_Lab2
{
    public interface ICrypter
    {
        ICrypter Create(byte[] key);
        byte[] Encrypt(byte[] source);
        byte[] Decrypt(byte[] source);
        int BlockSize { get; }
        int KeySize { get; }
    }
}