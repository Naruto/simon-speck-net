using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Speck
{

    internal static class Utils
    {
        private static RNGCryptoServiceProvider rng = null;

        public static RNGCryptoServiceProvider getRNG()
        {
            if (rng == null)
            {
                rng = new RNGCryptoServiceProvider();
            }
            return rng;
        }
    }

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
    
    public class Speck : SymmetricAlgorithm
    {
        private static KeySizes[] legalBlocKeySizeses =
        {
            new KeySizes(128, 128, 0), 
        };

        private static KeySizes[] legalKeySizes =
        {
            new KeySizes(128, 128, 0),
        };

        public Speck()
        {
            KeySizeValue = 128;
            BlockSizeValue = 128;
            FeedbackSizeValue = BlockSizeValue;
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
            byte[] data = new byte[KeySizeValue / 8];
            Utils.getRNG().GetBytes(data);
            IV = data;
        }

        public override void GenerateKey()
        {
            byte[] data = new byte[KeySizeValue / 8];
            Utils.getRNG().GetBytes(data);
            Key = data;
        }
    }

    internal class SpeckContext : IDisposable
    {
        private const string LibraryName = "Speck";
        private IntPtr _ctx = IntPtr.Zero;

        [DllImport(LibraryName)]  
        private static extern IntPtr speck_init(int _type, byte[] key, int key_len);

        [DllImport(LibraryName)]  
        private static extern void speck_encrypt(IntPtr ctx, IntPtr plaintext, IntPtr ciphertext);

        [DllImport(LibraryName)]          
        private static extern void speck_decrypt(IntPtr ctx, IntPtr ciphertext, IntPtr decrypted);

        [DllImport(LibraryName)]          
        private static extern int speck_ecb_encrypt(IntPtr ctx, IntPtr _in, IntPtr _out, int len);

        [DllImport(LibraryName)]
        private static extern int speck_ecb_decrypt(IntPtr ctx, IntPtr _in, IntPtr _out, int len);

        [DllImport(LibraryName)]
        private static extern void speck_finish(IntPtr ctx);
        
        public SpeckContext(byte[] rgbKey)
        {
            if (rgbKey == null || rgbKey.Length <= 0)
            {
                throw new ArgumentException();
            }
            
            _ctx = speck_init(7, rgbKey, rgbKey.Length); // 7: SPECK_ENCRYPT_TYPE_128_128
            if (_ctx == IntPtr.Zero)
            {
                throw new ArgumentException();                
            }
        }

        public void Dispose()
        {
            if (_ctx == IntPtr.Zero) return;
            speck_finish(_ctx);
            _ctx = IntPtr.Zero;
        }

        public unsafe int EncryptECB(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset)
        {
            int r;
            fixed (byte* _in = &inputBuffer[inputOffset], _out = &outBuffer[outOffset])
            {
                r = speck_ecb_encrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount);
            }
            return r;
        }
        
        public unsafe int DecryptECB(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset)
        {
            int r;
            fixed (byte* _in = &inputBuffer[inputOffset], _out = &outBuffer[outOffset])
            {
                r = speck_ecb_decrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount);
            }
            return r;
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
                throw new ArgumentNullException(nameof(inputBuffer));
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputOffset));
            }
            if (inputCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount % InputBlockSize != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (outputBuffer == null) {
                throw new ArgumentNullException(nameof(outputBuffer));
            }
            if (inputCount > outputBuffer.Length - outputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(outputOffset));
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
                throw new ArgumentNullException(nameof(inputBuffer));
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputOffset));
            }
            if (inputCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }

            byte[] outData = null;
            byte[] tmpData = Pad.PadBlock(_paddingMode, _blockSize, inputBuffer, inputOffset, inputCount);
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
                throw new ArgumentNullException(nameof(inputBuffer));
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputOffset));
            }
            if (inputCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount % InputBlockSize != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (outputBuffer == null) {
                throw new ArgumentNullException(nameof(outputBuffer));
            }
            if (inputCount > outputBuffer.Length - outputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(outputOffset));
            }

            return DecryptBlocks(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException(nameof(inputBuffer));
            }
            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputOffset));
            }
            if (inputCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
            }
            if (inputCount > inputBuffer.Length - inputOffset)
            {
                throw new ArgumentOutOfRangeException(nameof(inputCount));
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