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
                int bytesSent = 0;

                try
                {
                    package.Shrink(out buffer, true);

                    if (buffer.Length != 0)
                    {
                        // 1. The length ............. size of int
                        bytes = System.BitConverter.GetBytes(buffer.Length);

                        bytesSent = 0;
                        do
                        {
                            bytesSent += _socket.Send(bytes, bytesSent, sizeof(int) - bytesSent, SocketFlags.None);
                        }
                        while(bytesSent < sizeof(int));

                        // 2. The compressed flag .... size of bool
                        bytes = System.BitConverter.GetBytes(true);

                        bytesSent = 0;
                        do
                        {
                            bytesSent += _socket.Send(bytes, bytesSent, sizeof(bool) - bytesSent, SocketFlags.None);
                        }
                        while (bytesSent < sizeof(bool));

                        // 3. Data ................... length

                        bytesSent = 0;
                        do
                        {
                            bytesSent += _socket.Send(buffer, bytesSent, buffer.Length - bytesSent, SocketFlags.None);
                        }
                        while (bytesSent < buffer.Length);
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
                int length = 0;
                int bytesReceived = 0;
                byte[] buffer = null;
                byte[] bytes = new byte[8];
                bool bCompressed;

                byte[] originBuffer = null;

                try
                {
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
                        bytesReceived += _socket.Receive(bytes, bytesReceived, sizeof(bool) - bytesReceived, SocketFlags.None);
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

                    // Fill package
                    if (bCompressed)   // Try to uncompress data
                    {
                        originBuffer = ZlibUtilities.Inflate(buffer);
                        package = new NetBuffer(originBuffer.Length);
                        package.Write(originBuffer);
                    }
                    else
                    {
                        package = new NetBuffer(length);
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