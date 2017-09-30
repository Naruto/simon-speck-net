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