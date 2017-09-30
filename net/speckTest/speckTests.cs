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
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using SimonSpeckNet.Speck;

namespace speckTest
{
    [TestFixture]
    public class SpeckTests
    {
        private readonly byte[] _key128128 = new byte[16]    { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
        private readonly byte[] _plain128128 = new byte[16]  { 0x20, 0x6d, 0x61, 0x64, 0x65, 0x20, 0x69, 0x74, 0x20, 0x65, 0x71, 0x75, 0x69, 0x76, 0x61, 0x6c };
        private readonly byte[] _encrypt128128 = new byte[16] { 0x18, 0x0D, 0x57, 0x5C, 0xDF, 0xFE, 0x60, 0x78, 0x65, 0x32, 0x78, 0x79, 0x51, 0x98, 0x5D, 0xA6 };
        private readonly byte[] _key128192 = new byte[24]    { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17 };
        private readonly byte[] _plain128192 = new byte[16]  { 0x65, 0x6e, 0x74, 0x20, 0x74, 0x6f, 0x20, 0x43, 0x68, 0x69, 0x65, 0x66, 0x20, 0x48, 0x61, 0x72 };
        private readonly byte[] _encrypt128192 = new byte[16] { 0x86, 0x18, 0x3c, 0xe0, 0x5d, 0x18, 0xbc, 0xf9, 0x66, 0x55, 0x13, 0x13, 0x3a, 0xcf, 0xe4, 0x1b };
        private readonly byte[] _key128256 = new byte[32]    { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f };
        private readonly byte[] _plain128256 = new byte[16]  { 0x70, 0x6f, 0x6f, 0x6e, 0x65, 0x72, 0x2e, 0x20, 0x49, 0x6e, 0x20, 0x74, 0x68, 0x6f, 0x73, 0x65 };
        private readonly byte[] _encrypt128256 = new byte[16] { 0x43, 0x8f, 0x18, 0x9c, 0x8d, 0xb4, 0xee, 0x4e, 0x3e, 0xf5, 0xc0, 0x05, 0x04, 0x01, 0x09, 0x41 };
        private readonly int testNum = 256;

        [Test]
        public void SpeckInit()
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                }

                using (ICryptoTransform decryptor = algo.CreateDecryptor())
                {
                }
            }
        }


        private byte[] getRandom(int num)
        {
            Random random = new System.Random();
            byte[] byteArray = new byte[num];

            random.NextBytes(byteArray);
            return byteArray;
        }
        
        private bool CheckEncryptDecrypt128128(PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = _key128128;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    byte[] encrypted = new byte[16];
                    Utils.Encrypt(encryptor, _plain128128, encrypted);

                    for (int i = 0; i < _encrypt128128.Length; i++)
                    {
                        if (_encrypt128128[i] != encrypted[i])
                        {
                            return false;
                        }
                    }
                }
                
                using (ICryptoTransform decryptor = algo.CreateDecryptor())
                {
                    byte[] decrypted = new byte[16];
                    Utils.Decrypt(decryptor, _encrypt128128, decrypted);

                    for (int i = 0; i < _plain128128.Length; i++)
                    {
                        if (_plain128128[i] != decrypted[i])
                        {
                            Console.WriteLine(BitConverter.ToString(decrypted));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckEncryptDecrypt128128Stream(byte[] plain, PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = _key128128;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor(), decryptor = algo.CreateDecryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

                    for (int i = 0; i < plain.Length; i++)
                    {
                        if (plain[i] != decrypted[i])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        
        private bool CheckEncryptDecrypt128192(PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 192;
                algo.Key = _key128192;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    byte[] encrypted = new byte[16];
                    Utils.Encrypt(encryptor, _plain128192, encrypted);

                    for (int i = 0; i < _encrypt128192.Length; i++)
                    {
                        if (_encrypt128192[i] != encrypted[i])
                        {
                            return false;
                        }
                    }
                }
                
                using (ICryptoTransform decryptor = algo.CreateDecryptor())
                {
                    byte[] decrypted = new byte[16];
                    Utils.Decrypt(decryptor, _encrypt128192, decrypted);

                    for (int i = 0; i < _plain128192.Length; i++)
                    {
                        if (_plain128192[i] != decrypted[i])
                        {
                            Console.WriteLine(BitConverter.ToString(decrypted));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckEncryptDecrypt128192Stream(byte[] plain, PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 192;
                algo.Key = _key128192;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor(), decryptor = algo.CreateDecryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

                    for (int i = 0; i < plain.Length; i++)
                    {
                        if (plain[i] != decrypted[i])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckEncryptDecrypt128256(PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 256;
                algo.Key = _key128256;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    byte[] encrypted = new byte[16];
                    Utils.Encrypt(encryptor, _plain128256, encrypted);

                    for (int i = 0; i < _encrypt128256.Length; i++)
                    {
                        if (_encrypt128256[i] != encrypted[i])
                        {
                            return false;
                        }
                    }
                }
                
                using (ICryptoTransform decryptor = algo.CreateDecryptor())
                {
                    byte[] decrypted = new byte[16];
                    Utils.Decrypt(decryptor, _encrypt128256, decrypted);

                    for (int i = 0; i < _plain128256.Length; i++)
                    {
                        if (_plain128256[i] != decrypted[i])
                        {
                            Console.WriteLine(BitConverter.ToString(decrypted));
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckEncryptDecrypt128256Stream(byte[] plain, PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new Speck())
            {
                algo.BlockSize = 128;
                algo.KeySize = 256;
                algo.Key = _key128256;
                algo.Padding = paddingMode;

                using (ICryptoTransform encryptor = algo.CreateEncryptor(), decryptor = algo.CreateDecryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

                    for (int i = 0; i < plain.Length; i++)
                    {
                        if (plain[i] != decrypted[i])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        
        // 128/128
        
        [Test]
        public void Speck128128None()
        {
            bool result = CheckEncryptDecrypt128128(PaddingMode.None);
            Assert.True(result);
        }

        [Test]
        public void Speck128128Zeros()
        {
            bool result = CheckEncryptDecrypt128128(PaddingMode.Zeros);
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128BlocksStreamNone()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.None);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        [Test]
        public void Speck128128BlocksStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128BlockStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128BlockStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128BlockStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        
        [Test]
        public void Speck128128ByteStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128ByteStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128ByteStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128128ByteStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128128Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        // 128/192
        
        [Test]
        public void Speck128192None()
        {
            bool result = CheckEncryptDecrypt128192(PaddingMode.None);
            Assert.True(result);
        }

        [Test]
        public void Speck128192Zeros()
        {
            bool result = CheckEncryptDecrypt128192(PaddingMode.Zeros);
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192BlocksStreamNone()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.None);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        [Test]
        public void Speck128192BlocksStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192BlockStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192BlockStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192BlockStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        
        [Test]
        public void Speck128192ByteStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192ByteStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192ByteStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128192ByteStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128192Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }


        // 128/256
        
        [Test]
        public void Speck128256None()
        {
            bool result = CheckEncryptDecrypt128256(PaddingMode.None);
            Assert.True(result);
        }

        [Test]
        public void Speck128256Zeros()
        {
            bool result = CheckEncryptDecrypt128256(PaddingMode.Zeros);
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256BlocksStreamNone()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.None);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        [Test]
        public void Speck128256BlocksStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256BlockStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256BlockStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256BlockStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom(16 * (i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

        
        [Test]
        public void Speck128256ByteStreamZeros()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.Zeros);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256ByteStreamANSIX923()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.ANSIX923);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256ByteStreamISO10126()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.ISO10126);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }
        
        [Test]
        public void Speck128256ByteStreamPKCS7()
        {
            bool result = true;
            byte[] plain;
            for (int i = 0; i < testNum; i++)
            {
                plain = getRandom((i+1));
                result = CheckEncryptDecrypt128256Stream(plain, PaddingMode.PKCS7);
                if (result == false)
                    break;
            }
            Assert.True(result);
        }

    }
}