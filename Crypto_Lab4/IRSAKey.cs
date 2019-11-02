using System.IO;
using System.Numerics;
using Newtonsoft.Json;

namespace Crypto_Lab4
{
    public interface IRSAKey
    {
        BigInteger Exponent { get; }
        BigInteger Modulus { get; }
    }

    public class PrivateKey : IRSAKey
    {
        private const string fileName = "privateKey";

        public PrivateKey(BigInteger d, BigInteger n)
        {
            Exponent = d;
            Modulus = n;
        }

        [JsonIgnore]
        public BigInteger Exponent { get; }
        [JsonIgnore]
        public BigInteger Modulus { get; }

        [JsonProperty("Exponent")]
        private string exponent => Exponent.ToString();
        [JsonProperty("Modulus")]
        private string modulus => Modulus.ToString();

        public void Save()
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }

        public static PrivateKey Load()
        {
            dynamic obj = JsonConvert.DeserializeObject(File.ReadAllText(fileName));
            return new PrivateKey(BigInteger.Parse((string)obj.Exponent), BigInteger.Parse((string)obj.Modulus));
        }
    }

    public class PublicKey : IRSAKey
    {
        private const string fileName = "publicKey";

        public PublicKey(BigInteger e, BigInteger n)
        {
            Exponent = e;
            Modulus = n;
        }

        [JsonIgnore]
        public BigInteger Exponent { get; }
        [JsonIgnore]
        public BigInteger Modulus { get; }

        [JsonProperty("Exponent")]
        private string exponent => Exponent.ToString();
        [JsonProperty("Modulus")]
        private string modulus => Modulus.ToString();

        public void Save()
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }

        public static PublicKey Load()
        {
            dynamic obj = JsonConvert.DeserializeObject(File.ReadAllText(fileName));
            return new  PublicKey(BigInteger.Parse((string)obj.Exponent), BigInteger.Parse((string)obj.Modulus));
        }
    }
}