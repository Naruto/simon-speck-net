using System.Security.Cryptography;

namespace Speck
{
    internal static class Utils
    {
        private static RNGCryptoServiceProvider rng = null;

        public static RNGCryptoServiceProvider getRNG()
        {
            if (rng == null)
            {
                rng = new RNGCryptoServiceProvider();
            }
            return rng;
        }
    }
}