using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;


namespace TeamHub
{
    namespace Kernel
    {
        public class NetBuffer : NetDataPackage
        {
            #region Constructors
            public NetBuffer(int SizeInBytes)
            {
                _buffer = new byte[SizeInBytes];
                _streamWritePtr = 0;
                _streamReadPtr = 0;
                _count = 0;
            }
            #endregion

            #region Methods
            public uint GetWritePointer()
            {
                return _streamWritePtr;
            }

            public uint GetReadPointer()
            {
                return _streamReadPtr;
            }

            public void ResetWritePointer()
            {
                _streamWritePtr = 0;
                _count = 0;
            }

            public void ResetReadPointer()
            {
                _streamReadPtr = 0;
            }

            public void Seek_ReadPointerBegin(uint offset)
            {
                _streamReadPtr = offset;
            }

            public void Seek_ReadPointerCur(uint offset)
            {
                _streamReadPtr += offset;
            }

            public void Seek_ReadPointerEnd(uint offset)
            {
                _streamReadPtr = (uint)_buffer.Length - offset;
            }
            #endregion

            #region Implementations
            public override int SizeInBytes()
            {
                return _buffer.Length;
            }

            public override uint Count()
            {
                return _count;
            }

            public override uint StringLength()
            {
                return _streamWritePtr / 2;
            }

            public override void Clear()
            {
                try
                {
                    for (int i = 0; i < _buffer.Length; ++i)
                        _buffer[i] = 0;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                finally
                {
                    _streamWritePtr = 0;
                    _streamReadPtr = 0;
                    _count = 0;
                }
            }

            public override void Resize(int SizeInBytes)
            {
                _buffer = new byte[SizeInBytes];
                _streamWritePtr = 0;
                _streamReadPtr = 0;
                _count = 0;
            }

            public override void Shrink(out byte[] buffer, bool bCompress)
            {
                try
                {
                    buffer = new byte[_streamWritePtr];

                    for (int i = 0; i < _streamWritePtr; ++i)
                    {
                        buffer[i] = _buffer[i];
                        ++i;
                    }


                    if (bCompress)
                    {                        
                        byte[] compressedBuffer;

                        compressedBuffer = ZlibUtilities.Deflate(buffer);
                        buffer = compressedBuffer;
                    }
                }
                catch(Exception exception)
                {
                    throw exception;
                }
            }

            public override void Buffer(out byte[] buffer)
            {
                try
                {
                    buffer = _buffer;
                }
                catch(Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(int data)
            {
                byte[] convertedBytes;

                try
                {
                    convertedBytes = System.BitConverter.GetBytes(data);

                    foreach (byte elt in convertedBytes)
                    {
                        _buffer[_streamWritePtr++] = elt;
                    }

                    ++_count;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(int[] data)
            {
                byte[] convertedBytes;

                try
                {
                    foreach (int eltData in data)
                    {
                        convertedBytes = System.BitConverter.GetBytes(eltData);

                        foreach (byte elt in convertedBytes)
                        {
                            _buffer[_streamWritePtr++] = elt;
                        }
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(char data)
            {
                byte[] convertedBytes;

                try
                {
                    convertedBytes = System.BitConverter.GetBytes(data);

                    foreach (byte elt in convertedBytes)
                    {
                        _buffer[_streamWritePtr++] = elt;
                    }
                    ++_count;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(char[] data)
            {
                byte[] convertedBytes;

                try
                {
                    foreach (int eltData in data)
                    {
                        convertedBytes = System.BitConverter.GetBytes(eltData);

                        foreach (byte elt in convertedBytes)
                        {
                            _buffer[_streamWritePtr++] = elt;
                        }
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(byte data)
            {
                try
                {
                    _buffer[_streamWritePtr++] = data;
                    ++_count;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(byte[] data)
            {
                try
                {
                    foreach (byte ele in data)
                    {
                        _buffer[_streamWritePtr++] = ele;
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(float data)
            {
                byte[] convertedBytes;

                try
                {
                    convertedBytes = System.BitConverter.GetBytes(data);

                    foreach (byte elt in convertedBytes)
                    {
                        _buffer[_streamWritePtr++] = elt;
                    }
                    ++_count;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(float[] data)
            {
                byte[] convertedBytes;

                try
                {
                    foreach (int eltData in data)
                    {
                        convertedBytes = System.BitConverter.GetBytes(eltData);

                        foreach (byte elt in convertedBytes)
                        {
                            _buffer[_streamWritePtr++] = elt;
                        }
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(double data)
            {
                byte[] convertedBytes;

                try
                {
                    convertedBytes = System.BitConverter.GetBytes(data);

                    foreach (byte elt in convertedBytes)
                    {
                        _buffer[_streamWritePtr++] = elt;
                    }
                    ++_count;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(double[] data)
            {
                byte[] convertedBytes;

                try
                {
                    foreach (int eltData in data)
                    {
                        convertedBytes = System.BitConverter.GetBytes(eltData);

                        foreach (byte elt in convertedBytes)
                        {
                            _buffer[_streamWritePtr++] = elt;
                        }
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Write(string data)
            {
                byte[] convertedBytes;
                try
                {
                    foreach (char character in data)
                    {
                        convertedBytes = System.BitConverter.GetBytes(character);

                        foreach (byte elt in convertedBytes)
                        {
                            _buffer[_streamWritePtr++] = elt;                           
                        }
                        ++_count;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }


            public override void Read(out int data)
            {
                try
                {
                    data = System.BitConverter.ToInt32(_buffer, (int)_streamReadPtr);
                    _streamReadPtr += sizeof(int);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out int[] data, uint count)
            {
                data = new int[count];
                try
                {
                    for (int i = 0; i < count; ++i)
                    {
                        data[i] = System.BitConverter.ToInt32(_buffer, (int)_streamReadPtr);
                        _streamReadPtr += sizeof(int);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out char data)
            {
                try
                {
                    data = System.BitConverter.ToChar(_buffer, (int)_streamReadPtr);
                    _streamReadPtr += sizeof(char);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out char[] data, uint count)
            {
                data = new char[count];
                try
                {
                    for (int i = 0; i < count; ++i)
                    {
                        data[i] = System.BitConverter.ToChar(_buffer, (int)_streamReadPtr);
                        _streamReadPtr += sizeof(char);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out byte data)
            {
                try
                {
                    data = _buffer[_streamReadPtr];
                    _streamReadPtr += sizeof(byte);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out byte[] data, uint count)
            {
                data = new byte[count];
                try
                {
                    for (int i = 0; i < count; ++i)
                    {
                        data[i] = _buffer[_streamReadPtr++];
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out float data)
            {
                try
                {
                    data = System.BitConverter.ToSingle(_buffer, (int)_streamReadPtr);
                    _streamReadPtr += sizeof(float);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out float[] data, uint count)
            {
                data = new float[count];
                try
                {
                    for (int i = 0; i < count; ++i)
                    {
                        data[i] = System.BitConverter.ToSingle(_buffer, (int)_streamReadPtr);
                        _streamReadPtr += sizeof(float);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out double data)
            {
                try
                {
                    data = System.BitConverter.ToSingle(_buffer, (int)_streamReadPtr);
                    _streamReadPtr += sizeof(double);
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out double[] data, uint count)
            {
                data = new double[count];
                try
                {
                    for (int i = 0; i < count; ++i)
                    {
                        data[i] = System.BitConverter.ToDouble(_buffer, (int)_streamReadPtr);
                        _streamReadPtr += sizeof(double);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            public override void Read(out string data, uint length)
            {
                data = String.Empty;
                char tempChar;
                try
                {
                    for (int i = 0; i < length; ++i)
                    {
                        tempChar = System.BitConverter.ToChar(_buffer, (int)_streamReadPtr);
                        _streamReadPtr += sizeof(char);

                        data += tempChar;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            #endregion

            #region Properties
            byte[] _buffer;
            uint _streamWritePtr;
            uint _streamReadPtr;
            uint _count;
            #endregion
        }
    }
}