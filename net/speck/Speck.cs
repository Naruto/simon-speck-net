using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Speck
{
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
            this.PaddingValue = PaddingMode.None;
        }
        
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new SpeckDecryptoTransform(rgbKey, rgbIV);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new SpeckEncryptoTransform(rgbKey, rgbIV);
        }

        public override void GenerateIV()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[KeySizeValue / 8];
            rng.GetBytes(data);
            IV = data;
        }

        public override void GenerateKey()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] data = new byte[KeySizeValue / 8];
            rng.GetBytes(data);
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
        private static extern void speck_finish(out IntPtr ctx);
        
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
            // speck_finish(out _ctx);
            _ctx = IntPtr.Zero;
        }

        public unsafe int EncryptECB(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset)
        {
            // TODO: argument check

            int r;
            fixed (byte* _in = &inputBuffer[inputOffset])
            {
                fixed (byte* _out = &outBuffer[outOffset])
                {
                    r = speck_ecb_encrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount);
                }
            }
            return r;
        }
        
        public unsafe int DecryptECB(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset)
        {
            // TODO: argument check

            int r;
            fixed (byte* _in = &inputBuffer[inputOffset])
            {
                fixed (byte* _out = &outBuffer[outOffset])
                {
                    r = speck_ecb_decrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount);
                }
            }

            return r;
        }
    }
    
    public class SpeckDecryptoTransform : ICryptoTransform
    {
        private SpeckContext _speckContext;
        
        public SpeckDecryptoTransform(byte[] rgbKey, byte[] rgbIV)
        {
            _speckContext = new SpeckContext(rgbKey);
            if (_speckContext == null)
            {
                throw new ArgumentException();
            }
        }
        
        public void Dispose()
        {
            _speckContext.Dispose();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            // TODO: argument check
            
            // TODO: padding check

            int r = _speckContext.DecryptECB(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
            if (r != 0)
            {
                return -1;
            }
            
            return 0;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            // TODO: argument check
            
            // TODO: padding check

            byte[] decryptBuffer = new byte[inputCount];

            int r = _speckContext.DecryptECB(inputBuffer, inputOffset, inputCount, ref decryptBuffer, 0);
            if (r != 0)
            {
                return null;
            }
            
            // TODO: depadd check
            return decryptBuffer;
        }

        public bool CanReuseTransform { get { return true;} }
        public bool CanTransformMultipleBlocks { get { return true;} }
        public int InputBlockSize { get; }
        public int OutputBlockSize { get; }
    }

    public class SpeckEncryptoTransform : ICryptoTransform
    {
        private SpeckContext _speckContext;
        
        public SpeckEncryptoTransform(byte[] rgbKey, byte[] rgbIV)
        {
            _speckContext = new SpeckContext(rgbKey);
            if (_speckContext == null)
            {
                throw new ArgumentException();
            }
        }
        
        public void Dispose()
        {
            _speckContext.Dispose();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            // TODO: argument check
            
            // TODO: padding check

            int r = _speckContext.EncryptECB(inputBuffer, inputOffset, inputCount, ref outputBuffer, outputOffset);
            if (r != 0)
            {
                return -1;
            }
            
            return 0;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            // TODO: argument check
            
            // TODO: padding check

            byte[] decryptBuffer = new byte[inputCount];

            int r = _speckContext.EncryptECB(inputBuffer, inputOffset, inputCount, ref decryptBuffer, 0);
            if (r != 0)
            {
                return null;
            }
            
            // TODO: depadd check
            return decryptBuffer;
        }

        public bool CanReuseTransform { get { return true;} }
        public bool CanTransformMultipleBlocks { get { return true;} }
        public int InputBlockSize { get; }
        public int OutputBlockSize { get; }

    }
}