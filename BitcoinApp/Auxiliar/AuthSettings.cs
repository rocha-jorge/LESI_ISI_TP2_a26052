using System.Security.Cryptography;

namespace BitcoinApp.Auxiliar
{
    public class AuthSettings
    {
        public static string PrivateKey { get; set; } = GeraKey(256);

        public static string GeraKey(int nBytes)
        {
            Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.KeySize = 256;
            aesAlgorithm.GenerateKey();
            string keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
            return keyBase64;
        }
    }
}
