using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SampleStoreApp.Models
{
    public static class Helpers
    {
        public const string Netgiro_AppKey = "NETGIRO_APPKEY";
        public const string Netgiro_Signature = "NETGIRO_SIGNATURE";
        public const string Netgiro_Nonce = "NETGIRO_NONCE";

        public static string CalculateSignature(params string[] args)
        {
            string input = string.Join("", args);
            var sha = new SHA256CryptoServiceProvider();
            var hashArray = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            return hashArray.Aggregate(string.Empty, (current, b) => current + b.ToString("x2"));
        }
    }
}
