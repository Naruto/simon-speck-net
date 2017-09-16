using System;
using System.Security.Cryptography;

namespace Speck.Internal
{
    internal static class Pad
    {
        public static byte[] PadBlock(PaddingMode paddingMode, int inputBlockSize, byte[] block, int offset, int count)
        {
            byte[] result = null;
            int padBytes = inputBlockSize - (count % inputBlockSize);

            switch (paddingMode)
            {
                case PaddingMode.ANSIX923:
                    result = new byte[count + padBytes];
                    Buffer.BlockCopy(block, 0, result, 0, count);
                    result[result.Length - 1] = (byte) padBytes;
                    break;
                case PaddingMode.ISO10126:
                    result = new byte[count + padBytes];
                    byte[] random = new byte[result.Length - 1];
                    Utils.getRNG().GetBytes(random);
                    Buffer.BlockCopy(random, 0, result, 0, result.Length - 1);
                    Buffer.BlockCopy(block, 0, result, 0, count);
                    result[result.Length - 1] = (byte) padBytes;
                    break;
                case PaddingMode.None:
                    if (count % inputBlockSize != 0)
                    {
                        throw new CryptographicException();
                    }
                    result = new byte[count];
                    Buffer.BlockCopy(block, offset, result, 0, result.Length);
                    break;
                case PaddingMode.PKCS7:
                    result = new byte[count + padBytes];
                    Buffer.BlockCopy(block, offset, result, 0, count);

                    for (int i = count; i < result.Length; i++) {
                        result[i] = (byte)padBytes;
                    }
                    break;
                case PaddingMode.Zeros:
                    if (padBytes == inputBlockSize)
                    {
                        padBytes = 0;
                    }
                    result = new byte[count + padBytes];
                    Buffer.BlockCopy(block, offset, result, 0, count);
                    break;
                default:
                    throw new CryptographicException();
            }
            
            return result;
        }
        
        public static byte[] DepadBlock(PaddingMode paddingMode, int inputBlockSize, byte[] block, int offset, int count) {
            int padBytes = 0;

            switch (paddingMode) {
                case PaddingMode.ANSIX923:
                    padBytes = block[offset + count - 1];

                    if (padBytes <= 0 || padBytes > inputBlockSize)
                    {
                        throw new CryptographicException();
                    }

                    for (int i = offset + count - padBytes; i < offset + count - 1; i++) {
                        if (block[i] != 0)
                        {
                            throw new CryptographicException();
                        }
                    }

                    break;

                case PaddingMode.ISO10126:
                    padBytes = block[offset + count - 1];

                    if (padBytes <= 0 || padBytes > inputBlockSize)
                    {
                        throw new CryptographicException();
                    }

                    break;

                case PaddingMode.PKCS7:
                    padBytes = block[offset + count - 1];

                    if (padBytes <= 0 || padBytes > inputBlockSize)
                    {
                        throw new CryptographicException();
                    }

                    for (int i = offset + count - padBytes; i < offset + count; i++) {
                        if (block[i] != padBytes)
                        {
                            throw new CryptographicException();
                        }
                    }

                    break;

                case PaddingMode.Zeros:
                case PaddingMode.None:
                    padBytes = 0;
                    break;

                default:
                    throw new CryptographicException();
            }

            byte[] depadded = new byte[count - padBytes];
            Buffer.BlockCopy(block, offset, depadded, 0, depadded.Length);
            return depadded;
        }
    }
}