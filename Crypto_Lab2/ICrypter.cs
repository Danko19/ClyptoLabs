namespace Crypto_Lab2
{
    public interface ICrypter
    {
        byte[] Encrypt(byte[] source);
        byte[] Decrypt(byte[] source);
        int BlockSize { get; }
        int KeySize { get; }
    }
}