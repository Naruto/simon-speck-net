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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SimonSpeckNet.Speck;
using SimonSpeckNet.SpeckSteram;

namespace speckCLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            // speck ecb 128/128 
            Console.WriteLine("Speck ECB 128/128");
            using (SymmetricAlgorithm algo = new Speck())
            {
                byte[] key = new byte[16]   { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x20, 0x6d, 0x61, 0x64, 0x65, 0x20, 0x69, 0x74, 0x20, 0x65, 0x71, 0x75, 0x69, 0x76, 0x61, 0x6c };
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            
            // speck ctr 128/128
            Console.WriteLine("Speck CTR 128/128");
            using (SymmetricAlgorithm algo = new SpeckCTR())
            {
                byte[] key = new byte[16]   { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x20, 0x6d, 0x61, 0x64, 0x65, 0x20, 0x69, 0x74, 0x20, 0x65, 0x71, 0x75, 0x69, 0x76, 0x61, 0x6c };
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("IV:      " + BitConverter.ToString(algo.IV));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("IV:       " + BitConverter.ToString(algo.IV));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            // speck ecb 128/192 
            Console.WriteLine("Speck ECB 128/192");
            using (SymmetricAlgorithm algo = new Speck())
            {
                byte[] key = new byte[24]
                {
                    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17
                };
                algo.BlockSize = 128;
                algo.KeySize = 192;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x65, 0x6e, 0x74, 0x20, 0x74, 0x6f, 0x20, 0x43, 0x68, 0x69, 0x65, 0x66, 0x20, 0x48, 0x61, 0x72 };
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            // speck ctr 128/192
            Console.WriteLine("Speck CTR 128/192");
            using (SymmetricAlgorithm algo = new SpeckCTR())
            {
                byte[] key = new byte[24]
                {
                    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17
                };
                algo.BlockSize = 128;
                algo.KeySize = 192;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x65, 0x6e, 0x74, 0x20, 0x74, 0x6f, 0x20, 0x43, 0x68, 0x69, 0x65, 0x66, 0x20, 0x48, 0x61, 0x72};
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("IV:      " + BitConverter.ToString(algo.IV));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("IV:       " + BitConverter.ToString(algo.IV));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            // speck ecb 128/256 
            Console.WriteLine("Speck ECB 128/256");
            using (SymmetricAlgorithm algo = new Speck())
            {
                byte[] key = new byte[32]
                {
                    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f
                };
                algo.BlockSize = 128;
                algo.KeySize = 256;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x70, 0x6f, 0x6f, 0x6e, 0x65, 0x72, 0x2e, 0x20, 0x49, 0x6e, 0x20, 0x74, 0x68, 0x6f, 0x73, 0x65 };
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            // speck ctr 128/256
            Console.WriteLine("Speck CTR 128/256");
            using (SymmetricAlgorithm algo = new SpeckCTR())
            {
                byte[] key = new byte[32]
                {
                    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f
                };
                algo.BlockSize = 128;
                algo.KeySize = 256;
                algo.Key = key;
                
                algo.GenerateIV();
                using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
                {
                    byte[] plain = new byte[16] { 0x70, 0x6f, 0x6f, 0x6e, 0x65, 0x72, 0x2e, 0x20, 0x49, 0x6e, 0x20, 0x74, 0x68, 0x6f, 0x73, 0x65 };
                    
                    byte[] plainEnc = encryptor.TransformFinalBlock(plain, 0, plain.Length);
                    byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
                    Console.WriteLine("key:     " + BitConverter.ToString(key));
                    Console.WriteLine("IV:      " + BitConverter.ToString(algo.IV));
                    Console.WriteLine("plain:   " + BitConverter.ToString(plain));
                    Console.WriteLine("encrypt: " + BitConverter.ToString(plainEnc));
                    Console.WriteLine("decrypt: " + BitConverter.ToString(plainDec));
                    Console.WriteLine();
                }
                
                algo.GenerateKey();
                using (ICryptoTransform encryptor = algo.CreateEncryptor())
                {
                    using (ICryptoTransform decryptor = algo.CreateDecryptor())
                    {
                        String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
                        byte[] aByte = ASCIIEncoding.ASCII.GetBytes(a);
                        byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
                        byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
                        Console.WriteLine("Key:      " + BitConverter.ToString(algo.Key));
                        Console.WriteLine("IV:       " + BitConverter.ToString(algo.IV));
                        Console.WriteLine("aByte:    " + BitConverter.ToString(aByte));
                        Console.WriteLine("aByteEnc: " + BitConverter.ToString(aByteEnc));
                        Console.WriteLine("aByteDec: " + BitConverter.ToString(aByteDec));
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine();
            
            // speck stream
            using (SymmetricAlgorithm algo = new Speck())
            {
                byte[] key = new byte[16]   { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = key;
                
                byte[] plain = new byte[16] { 0x20, 0x6d, 0x61, 0x64, 0x65, 0x20, 0x69, 0x74, 0x20, 0x65, 0x71, 0x75, 0x69, 0x76, 0x61, 0x6c };
                byte[] output;
                byte[] decrypt = new byte[16];

                // write(encrypt)
                {
                    MemoryStream ms = new MemoryStream();
                    using (SpeckStream stream = new SpeckStream(ms, algo, SpeckStreamMode.Write))
                    {
                        stream.Write(plain, 0, plain.Length);
                        stream.FlushFinalBlock();
                        output = ms.ToArray();
                        Console.WriteLine("encrypt: " + BitConverter.ToString(output));
                    }
                }

                //read(decrypt)
                {
                    MemoryStream ms = new MemoryStream(output);
                    using (SpeckStream stream = new SpeckStream(ms, algo, SpeckStreamMode.Read))
                    {
                        stream.Read(decrypt, 0, decrypt.Length);
                        Console.WriteLine("decrypt: " + BitConverter.ToString(decrypt));
                    }
                }
            }
            
            // speck stream seek
            using (SymmetricAlgorithm algo = new Speck())
            {
                byte[] key = new byte[16]   { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
                algo.BlockSize = 128;
                algo.KeySize = 128;
                algo.Key = key;

                int size = 1040;
                byte[] plain = new byte[size];
                byte[] output = new byte[size];
                byte[] decrypt = new byte[size];

                Random cRandom = new System.Random();
                cRandom.NextBytes(plain);
                
                // write(encrypt)
                {
                    MemoryStream ms = new MemoryStream();
                    using (SpeckStream stream = new SpeckStream(ms, algo, SpeckStreamMode.Write))
                    {
                        stream.Write(plain, 0, plain.Length - 16);
                        stream.Seek(1024, SeekOrigin.Begin);
                        stream.Write(plain, plain.Length - 16, 16);
                        stream.FlushFinalBlock();
                        output = ms.ToArray();
                        Console.WriteLine("encrypt: " + BitConverter.ToString(output));
                    }
                }

                //read(decrypt)
                {
                    MemoryStream ms = new MemoryStream(output);
                    using (SpeckStream stream = new SpeckStream(ms, algo, SpeckStreamMode.Read))
                    {
                        stream.Read(decrypt, 0, decrypt.Length);
                        Console.WriteLine("decrypt: " + BitConverter.ToString(decrypt));
                    }
                }
            }
        }
    }
}