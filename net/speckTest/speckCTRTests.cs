using System;
using System.Security.Cryptography;
using NUnit.Framework;
using SimonSpeckNet.Speck;

namespace speckTest
{
    [TestFixture]
    public class speckCTRTests
    {
        private readonly int testNum = 256;

        [Test]
        public void SpeckInit()
        {
            using (SymmetricAlgorithm algo = new SpeckCTR())
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

        private bool CheckEncryptDecrypt128128Stream(byte[] plain, PaddingMode paddingMode)
        {
            using (SymmetricAlgorithm algo = new SpeckCTR())
            {
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.GenerateKey();
                algo.GenerateIV();
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

    }
}