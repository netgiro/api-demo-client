using System.Security.Cryptography;

namespace NetgiroClient.Helpers
{
    public class RandomString
    {
        private const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Generate random string of letters.
        /// </summary>
        /// <param name="length">How many random characters will be generated</param>
        /// <returns>Random string of letters</returns>
        public static string Generate(int length = 16)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[length];
            provider.GetBytes(byteArray);

            string randomString = "";
            foreach (byte currentByte in byteArray)
            {
                randomString += validCharacters[currentByte / 5];
            }

            return randomString;
        }
    }
}
