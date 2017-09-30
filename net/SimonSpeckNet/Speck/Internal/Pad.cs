//
// Copyright (c) 2017 Naruto TAKAHASHI <tnaruto@gmail.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using System.Security.Cryptography;

namespace SimonSpeckNet.Speck
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