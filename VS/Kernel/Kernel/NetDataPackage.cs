using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeamHub
{
    namespace Kernel
    {
        public abstract class NetDataPackage
        {
            #region Constructors
            public NetDataPackage()
            { }
            #endregion

            #region Interfaces
            public abstract int SizeInBytes();
            public abstract uint Count();
            public abstract void Resize(int SizeInBytes);
            public abstract void Shrink(out byte[] buffer, bool bCompress);
            public abstract void Buffer(out byte[] buffer);
            public abstract void Clear();

            public abstract void Write(int data);
            public abstract void Write(int[] data);
            public abstract void Write(char data);
            public abstract void Write(char[] data);
            public abstract void Write(byte data);
            public abstract void Write(byte[] data);
            public abstract void Write(float data);
            public abstract void Write(float[] data);
            public abstract void Write(double data);
            public abstract void Write(double[] data);
            public abstract void Write(string data);

            public abstract void Read(out int data);
            public abstract void Read(out int[] data, uint count);
            public abstract void Read(out char data);
            public abstract void Read(out char[] data, uint count);
            public abstract void Read(out byte data);
            public abstract void Read(out byte[] data, uint count);
            public abstract void Read(out float data);
            public abstract void Read(out float[] data, uint count);
            public abstract void Read(out double data);
            public abstract void Read(out double[] data, uint count);
            public abstract void Read(out string data, uint length);
            #endregion
        }
    }
}