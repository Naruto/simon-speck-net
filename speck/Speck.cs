using System;
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
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }

    public class SpeckDecryptoTransform : ICryptoTransform
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        public bool CanReuseTransform { get; }
        public bool CanTransformMultipleBlocks { get; }
        public int InputBlockSize { get; }
        public int OutputBlockSize { get; }
    }

    public class SpeckEncryptoTransform : ICryptoTransform
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        public bool CanReuseTransform { get; }
        public bool CanTransformMultipleBlocks { get; }
        public int InputBlockSize { get; }
        public int OutputBlockSize { get; }
    }
}