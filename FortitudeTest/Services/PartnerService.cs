using System.Text;
using System.Security.Cryptography;

namespace FortitudeTest.Services
{
    public class PartnerService
    {
        private static readonly Dictionary<string, (string Name, string Password)> Partners = new()
        {
            { "FG-00001", ("FAKEGOOGLE", "FAKEPASSWORD1234") },
            { "FG-00002", ("FAKEPEOPLE", "FAKEPASSWORD4578") }
        };

        public static bool IsAuthorized(string partnerRefNo, string partnerKey, string passwordBase64)
        {
            if (!Partners.TryGetValue(partnerRefNo, out var partner))
                return false;

            var expectedPasswordBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(partner.Password));
            return partner.Name == partnerKey && passwordBase64 == expectedPasswordBase64;
        }

        public static string GenerateSignature(string timestamp, string partnerKey, string partnerRefNo, long totalAmount, string passwordBase64)
        {
            string concat = $"{timestamp}{partnerKey}{partnerRefNo}{totalAmount}{passwordBase64}";
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(concat));
            var hex = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(hex));
        }

    }
}
