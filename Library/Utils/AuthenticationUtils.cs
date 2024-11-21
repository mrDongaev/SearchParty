using System.Security.Cryptography;

namespace Library.Utils
{
    public static class AuthenticationUtils
    {
        public static RSAParameters LoadPrivateKeyFromPem(string privateKeyPem)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(privateKeyPem.ToCharArray());
                return rsa.ExportParameters(includePrivateParameters: true);
            }
        }

        public static RSAParameters LoadPublicKeyFromPem(string publicKeyPem)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKeyPem.ToCharArray());
                return rsa.ExportParameters(includePrivateParameters: false);
            }
        }
    }
}
