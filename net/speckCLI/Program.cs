using System;
using System.Security.Cryptography;
using System.Text;

namespace speckCLI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            // speck ecb 
            Console.WriteLine("Speck ECB");
            using (SymmetricAlgorithm algo = new Speck.Speck())
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
            
            
            // speck ctr 
            Console.WriteLine("Speck CTR");
            using (SymmetricAlgorithm algo = new Speck.SpeckCTR())
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
            
        }
    }
}