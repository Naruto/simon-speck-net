using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Speck;
using System.Security.Cryptography;
using System;
using System.Text;

public class SpeckSampleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// speck ecb
		Debug.Log("Speck ECB");
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
		Debug.Log("Speck CTR");
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
