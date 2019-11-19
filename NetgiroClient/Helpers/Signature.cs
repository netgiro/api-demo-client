using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetgiroClient.Helpers
{
    public static class Signature
    {
        /// <summary>
        /// Signature represents computed hash that both client and Netgíró have to create so other can verify if it came from authenticated source.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>HMACSHA256 computed hash</returns>
        public static string CalculateSignature(params string[] args)
        {
            string input = string.Join("", args);
            var sha = new SHA256CryptoServiceProvider();
            var hashArray = sha.ComputeHash(Encoding.UTF8.GetBytes(input));

            return hashArray.Aggregate(string.Empty, (current, b) => current + b.ToString("x2"));
        }
    }
}
