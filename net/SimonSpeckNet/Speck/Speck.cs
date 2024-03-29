﻿//
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
    public class Speck : SymmetricAlgorithm
    {
        private static KeySizes[] legalBlocKeySizeses =
        {
            new KeySizes(128, 128, 0), 
        };

        private static KeySizes[] legalKeySizes =
        {
            new KeySizes(128, 256, 64), 
        };

        public Speck()
        {
            KeySizeValue = 128;
            BlockSizeValue = 128;
            FeedbackSizeValue = BlockSizeValue;
            LegalBlockSizesValue = Speck.legalBlocKeySizeses;
            LegalKeySizesValue = Speck.legalKeySizes;
            
            this.ModeValue = CipherMode.ECB;
            this.PaddingValue = PaddingMode.PKCS7;
        }
        
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            
            return new SpeckDecryptoTransform(rgbKey, rgbIV, BlockSize, PaddingValue);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new SpeckEncryptoTransform(rgbKey, rgbIV, BlockSize, PaddingValue);
        }

        public override void GenerateIV()
        {
            byte[] data = new byte[BlockSizeValue / 8];
            Utils.getRNG().GetBytes(data);
            IVValue = data;
        }

        public override void GenerateKey()
        {
            byte[] data = new byte[KeySizeValue / 8];
            Utils.getRNG().GetBytes(data);
            KeyValue = data;
        }
    }
    
    public class SpeckEncryptoTransform : ICryptoTransform
    {
        private SpeckContext _speckContext;
        private readonly byte[] _iv;
        private readonly PaddingMode _paddingMode;
        private readonly int _blockSize;
        private byte[] _depadBuffer;
        
        public SpeckEncryptoTransform(byte[] rgbKey, byte[] rgbIV, int blockSize, PaddingMode paddingMode)
        {
            _speckContext = new SpeckContext(rgbKey);
            if (_speckContext == null)
            {
                throw new ArgumentException();
            }
            _iv = rgbIV; // not use
            _blockSize = blockSize;
            _paddingMode = paddingMode;
            _depadBuffer = null;
        }
        
        public void Dispose()
        {
            _speckContext.Dispose();
            if (_depadBuffer != null)
            {
                Array.Clear(_depadBuffer, 0, _depadBuffer.Length);
            }
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }
            if (inputCount <= 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount % InputBlockSize != 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (outputBuffer == null) {
                throw new ArgumentNullException("outputBuffer");
            }
            if (inputCount > outputBuffer.Length - outputOffset)
            {
                throw new ArgumentOutOfRangeException("outputOffset");
            }

            int r = _speckContext.EncryptECB(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
            if (r != 0)
            {
                return -1;
            }
            
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }
            if (inputCount < 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }

            byte[] outData = null;
            byte[] tmpData = Pad.PadBlock(_paddingMode, InputBlockSize, inputBuffer, inputOffset, inputCount);
            if (tmpData.Length > 0)
            {
                outData = new byte[tmpData.Length];
                int r = _speckContext.EncryptECB(tmpData, 0, tmpData.Length, ref outData, 0);
                if (r != 0)
                {
                    return null;
                }
            }
            
            Reset();
            
            return outData;
        }

        private void Reset()
        {
            if (_depadBuffer != null)
            {
                Array.Clear(_depadBuffer, 0, _depadBuffer.Length);
                _depadBuffer = null;
            }
        }

        public bool CanReuseTransform
        {
            get { return true; }
        }

        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        public int InputBlockSize
        {
            get { return _blockSize / 8; }
        }

        public int OutputBlockSize
        {
            get { return _blockSize / 8; }
        }
    }
    
    public class SpeckDecryptoTransform : ICryptoTransform
    {
        private SpeckContext _speckContext;
        private readonly byte[] _iv;
        private readonly PaddingMode _paddingMode;
        private readonly int _blockSize;
        private byte[] _depadBuffer;
        
        public SpeckDecryptoTransform(byte[] rgbKey, byte[] rgbIV, int blockSize, PaddingMode paddingMode)
        {
            _speckContext = new SpeckContext(rgbKey);
            if (_speckContext == null)
            {
                throw new ArgumentException();
            }
            _iv = rgbIV; // not use
            _blockSize = blockSize;
            _paddingMode = paddingMode;
        }
        
        public void Dispose()
        {
            _speckContext.Dispose();
            
            if (_depadBuffer != null)
            {
                Array.Clear(_depadBuffer, 0, _depadBuffer.Length);
            }
        }
        
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }
            if (inputCount <= 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount % InputBlockSize != 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (outputBuffer == null) {
                throw new ArgumentNullException("outputBuffer");
            }
            if (inputCount > outputBuffer.Length - outputOffset)
            {
                throw new ArgumentOutOfRangeException("outputOffset");
            }

            return DecryptBlocks(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }
            if (inputCount < 0)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }
            
            if (inputCount % InputBlockSize != 0)
            {
                throw new CryptographicException();
            }

            if (inputCount % InputBlockSize != 0)
            {
                throw new CryptographicException();
            }

            byte[] outputData = null;
            byte[] cipherText = null;

            if (_depadBuffer == null) {
                cipherText = new byte[inputCount];
                Buffer.BlockCopy(inputBuffer, inputOffset, cipherText, 0, inputCount);
            }
            else {
                cipherText = new byte[_depadBuffer.Length + inputCount];
                Buffer.BlockCopy(_depadBuffer, 0, cipherText, 0, _depadBuffer.Length);
                Buffer.BlockCopy(inputBuffer, inputOffset, cipherText, _depadBuffer.Length, inputCount);
                
            }

            // Decrypt the data, then strip the padding to get the final decrypted data.
            if (cipherText.Length > 0)
            {
                byte[] tmpData = new byte[cipherText.Length];
                int r = _speckContext.DecryptECB(cipherText, 0, cipherText.Length, ref tmpData, 0);
                if (r != 0)
                {
                    throw new CryptographicException();
                }
                outputData = Pad.DepadBlock(_paddingMode, InputBlockSize, tmpData, 0, tmpData.Length);
            }
            else
            {
                outputData = new byte[0];
            }

            Reset();

            return outputData;
        }

        private int DecryptBlocks(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outputBuffer, int outputOffset)
        {
            int decryptedBytes = 0;
            if (_paddingMode != PaddingMode.None && _paddingMode != PaddingMode.Zeros)
            {
                // If we have data saved from a previous call, decrypt that into the output first
                if (_depadBuffer != null)
                {
                    int r = _speckContext.DecryptECB(_depadBuffer, 0, _depadBuffer.Length, ref outputBuffer,
                        outputOffset);
                    if (r != 0)
                    {
                        throw new CryptographicException();
                    }
                    int depadDecryptLength = _depadBuffer.Length;
                    Array.Clear(_depadBuffer, 0, _depadBuffer.Length);
                    outputOffset += depadDecryptLength;
                    decryptedBytes += depadDecryptLength;
                }
                else
                {
                    _depadBuffer = new byte[InputBlockSize];
                }
                
                // Copy the last block of the input buffer into the depad buffer
                Buffer.BlockCopy(inputBuffer,
                    inputOffset + inputCount - _depadBuffer.Length,
                    _depadBuffer,
                    0,
                    _depadBuffer.Length);
                inputCount -= _depadBuffer.Length;
            }

            if (inputCount > 0)
            {
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
                int r = _speckContext.DecryptECB(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
                if (r != 0)
                {
                    throw new CryptographicException();
                }
                decryptedBytes += inputCount;
            }
            return decryptedBytes;
        }
        
        private void Reset()
        {
            if (_depadBuffer != null)
            {
                Array.Clear(_depadBuffer, 0, _depadBuffer.Length);
                _depadBuffer = null;
            }
        }
        
        public bool CanReuseTransform
        {
            get { return true; }
        }

        public bool CanTransformMultipleBlocks
        {
            get { return true; }
        }

        public int InputBlockSize
        {
            get { return _blockSize / 8; }
        }

        public int OutputBlockSize
        {
            get { return _blockSize / 8; }
        }
    }
}