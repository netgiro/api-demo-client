using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SampleStoreApp.Helpers
{
    public static class Signature
    {
        public static string CalculateSignature(params string[] args)
        {
            string input = string.Join("", args);
            var sha = new SHA256CryptoServiceProvider();
            var hashArray = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            return hashArray.Aggregate(string.Empty, (current, b) => current + b.ToString("x2"));
        }
    }
}
