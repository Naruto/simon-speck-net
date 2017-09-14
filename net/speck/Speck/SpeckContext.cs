using System;
using System.Runtime.InteropServices;

namespace Speck
{
    internal class SpeckContext : IDisposable
    {
#if __IOS__
        private const string LibraryName = "__Internal";
#elif UNITY_IOS
        private const string LibraryName = "__Internal";
#else
        private const string LibraryName = "speck";
#endif
        private IntPtr _ctx;

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
        private static extern int speck_ctr_encrypt(IntPtr ctx, IntPtr _in, IntPtr _out, int len, IntPtr iv, int iv_len);

        [DllImport(LibraryName)]
        private static extern int speck_ctr_decrypt(IntPtr ctx, IntPtr _in, IntPtr _out, int len, IntPtr iv, int iv_len);

        [DllImport(LibraryName)]
        private static extern void speck_finish(ref IntPtr ctx);
        
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
            speck_finish(ref _ctx);
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

        public unsafe int EncryptCTR(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset, ref byte[] iv)
        {
            int r;
            fixed (byte* _in = &inputBuffer[inputOffset], _out = &outBuffer[outOffset], _iv = iv)
            {
                r = speck_ctr_encrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount, new IntPtr(_iv), iv.Length);
            }
            return r;            
        }
        
        public unsafe int DecryptCTR(byte[] inputBuffer, int inputOffset, int inputCount, ref byte[] outBuffer, int outOffset, ref byte[] iv)
        {
            int r;
            fixed (byte* _in = &inputBuffer[inputOffset], _out = &outBuffer[outOffset], _iv = iv)
            {
                r = speck_ctr_decrypt(_ctx, new IntPtr(_in), new IntPtr(_out), inputCount, new IntPtr(_iv), iv.Length);
            }
            return r;            
        }
    }
}