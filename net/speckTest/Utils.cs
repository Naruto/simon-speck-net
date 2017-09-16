using System;
using System.Security.Cryptography;

namespace speckTest
{
    public static class Utils
    {
        public static void Encrypt(ICryptoTransform trans, byte[] input, byte[] output)
        {
            int bs = trans.InputBlockSize;
            int full = input.Length / bs;
            int partial = input.Length % bs;
            int pos = 0;
            for (int i=0; i < full; i++) {
                trans.TransformBlock (input, pos, bs, output, pos);
                pos += bs;
            }
            if (partial > 0) {
                byte[] final = trans.TransformFinalBlock (input, pos, partial);
                Array.Copy (final, 0, output, pos, bs);
            }
        }

        public static void Decrypt(ICryptoTransform trans, byte[] input, byte[] output)
        {
            int bs = trans.InputBlockSize;
            int full = input.Length / bs;
            int partial = input.Length % bs;
            int pos = 0;
            for (int i=0; i < full; i++) {
                trans.TransformBlock (input, pos, bs, output, pos);
                pos += bs;
            }
            if (partial > 0) {
                byte[] final = trans.TransformFinalBlock (input, pos, partial);
                Array.Copy (final, 0, output, pos, partial);
            }
        }   
    }
}