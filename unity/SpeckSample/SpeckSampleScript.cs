using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Text;
using SimonSpeckNet.Speck;

public class SpeckSampleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// speck ecb
		Debug.Log("Speck ECB 128/128");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		// speck ctr
		Debug.Log("Speck CTR 128/128");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("IV:      " + BitConverter.ToString(algo.IV));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("IV:       " + BitConverter.ToString(algo.IV));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		// speck ecb
		Debug.Log("Speck ECB 128/192");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		// speck ctr
		Debug.Log("Speck CTR 128/192");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("IV:      " + BitConverter.ToString(algo.IV));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("IV:       " + BitConverter.ToString(algo.IV));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		// speck ecb
		Debug.Log("Speck ECB 128/256");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		// speck ctr
		Debug.Log("Speck CTR 128/192");
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
				Debug.Log("key:     " + BitConverter.ToString(key));
				Debug.Log("IV:      " + BitConverter.ToString(algo.IV));
				Debug.Log("plain:   " + BitConverter.ToString(plain));
				Debug.Log("encrypt: " + BitConverter.ToString(plainEnc));
				Debug.Log("decrypt: " + BitConverter.ToString(plainDec));
				Debug.Log("");
			}

			algo.GenerateKey();
			using (ICryptoTransform encryptor = algo.CreateEncryptor())
			{
				using (ICryptoTransform decryptor = algo.CreateDecryptor())
				{
					String a = "hello, world! hello, world!! hello, world!!! hello, world!!!! hello, world!!!!!";
					byte[] aByte = Encoding.ASCII.GetBytes(a);
					byte[] aByteEnc = encryptor.TransformFinalBlock(aByte, 0, aByte.Length);
					byte[] aByteDec = decryptor.TransformFinalBlock(aByteEnc, 0, aByteEnc.Length);
					Debug.Log("Key:      " + BitConverter.ToString(algo.Key));
					Debug.Log("IV:       " + BitConverter.ToString(algo.IV));
					Debug.Log("aByte:    " + BitConverter.ToString(aByte));
					Debug.Log("aByteEnc: " + BitConverter.ToString(aByteEnc));
					Debug.Log("aByteDec: " + BitConverter.ToString(aByteDec));
					Debug.Log("");
				}
			}
		}
		Debug.Log("");

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
