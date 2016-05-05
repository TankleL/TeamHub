using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace TeamHub
{

    namespace Kernel
    {
        public class TCPClient : NetClient
        {
            #region Constructors
            public TCPClient(string ipAddress, int port)
            {
                _ipAddress = ipAddress;
                _port = port;                
            }
            #endregion

            #region Methods
            #endregion

            #region Implementations
            public override void Connect()
            {
                try
                {
                    IPAddress ipAddr = IPAddress.Parse(_ipAddress);
                    IPEndPoint endPoint = new IPEndPoint(ipAddr, _port);

                    _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    _socket.Connect(endPoint);
                }
                catch(Exception exception)
                {
                    throw exception;
                }
            }

            public override void Close()
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
            }

            /// <summary>
            ///     Formate:
            ///     -------------------- CommPack ---------------------
            ///     |   1. The length ............. size of int       |
            ///     |   2. The compressed flag .... size of bool      |
            ///     |   3. Data ................... length            |
            ///     ---------------------------------------------------
            /// </summary>
            /// <param name="package"></param>
            public override void Send(NetDataPackage package)
            {
                byte[] buffer;
                byte[] bytes;

                try
                {
                    package.Shrink(out buffer, true);

                    if (buffer.Length != 0)
                    {
                        // 1. The length ............. size of int
                        bytes = System.BitConverter.GetBytes(buffer.Length);
                        _socket.Send(bytes, sizeof(int), SocketFlags.None);

                        // 2. The compressed flag .... size of bool
                        bytes = System.BitConverter.GetBytes(true);
                        _socket.Send(bytes, sizeof(bool), SocketFlags.None);

                        // 3. Data ................... length
                        _socket.Send(buffer);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            /// <summary>
            ///     Formate:
            ///     -------------------- CommPack ---------------------
            ///     |   1. The length ............. size of int       |
            ///     |   2. The compressed flag .... size of bool      |
            ///     |   3. Data ................... length            |
            ///     ---------------------------------------------------
            /// </summary>
            /// <param name="package"></param>
            public override void Receive(out NetDataPackage package)
            {
                int sizeOfPack = 0;
                int length = 0;
                byte[] buffer = null;
                byte[] bytes = null;
                bool bCompressed;
                int bytesReceived = 0;

                byte[] originBuffer = null;

                try
                {

                    package = new NetBuffer(sizeOfPack);

                    // 1. The length ............. size of int
                    bytesReceived = 0;
                    do
                    {
                        bytesReceived += _socket.Receive(bytes, sizeof(int) - bytesReceived, SocketFlags.None);
                    }
                    while (bytesReceived < sizeof(int));

                    length = System.BitConverter.ToInt32(bytes, 0);


                    // 2. The compressed flag .... size of bool
                    bytesReceived = 0;
                    do
                    {
                        bytesReceived += _socket.Receive(bytes, sizeof(bool), SocketFlags.None);
                    }
                    while (bytesReceived < sizeof(bool));
                    bCompressed = System.BitConverter.ToBoolean(bytes, 0);


                    // 3. Data ................... length
                    bytesReceived = 0;
                    buffer = new byte[length];
                    do
                    {
                        bytesReceived += _socket.Receive(buffer, bytesReceived, length - bytesReceived, SocketFlags.None);
                    }
                    while (bytesReceived < length);

                    if (bCompressed)   // Try to uncompress data
                    {
                        originBuffer = ZlibUtilities.Inflate(buffer);
                        package.Write(originBuffer);
                    }
                    else
                    {
                        package.Write(buffer);
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            #endregion

            #region Properties
            private string  _ipAddress;
            private int     _port;
            private Socket  _socket;
            #endregion
        }
    }
}