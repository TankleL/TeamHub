using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Ionic.Zlib;


namespace TeamHub
{

    namespace Kernel
    {
        public class TCPServer : NetServer
        {
            #region Enumerations
            public enum ServerStates : int
            {
                CannotWork = 0,
                Idle,
                Listening,
                Accepted
            }
            #endregion

            #region Constructors
            public TCPServer(int port, int backlog)
            {
                _port = port;
                _backlog = backlog;
                IPAddress ipAd = IPAddress.Any;
                _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _mainSocket.Bind(new IPEndPoint(ipAd, _port));

                _state = ServerStates.Idle;
            }

            private TCPServer(Socket mainSocket, int port, ServerStates state)
            {
                _mainSocket = mainSocket;
                _backlog = 0;
                _port = port;
                _state = state;
            }
            #endregion

            #region Methods
            public ServerStates GetState()                
            {   
                return _state;
            }
            #endregion

            #region Thread Functions
            private void ListenThread()
            {
                ParameterizedThreadStart parStart = new ParameterizedThreadStart(AcceptThread);
                Socket acceptedSocket;
                Thread acceptThread;

                try
                {
                    _mainSocket.Listen(_backlog);

                    // Fot test
                    Console.WriteLine("Listening...");
                    // End Of "For test"

                    while (true)
                    {
                        acceptedSocket = _mainSocket.Accept();

                        acceptThread = new Thread(parStart);
                        acceptThread.IsBackground = true;
                        acceptThread.Start(acceptedSocket);
                    }
                }
                catch(Exception exception)
                {
                    throw exception;
                }
            }

            private void AcceptThread(object param)
            {
                Socket socket = param as Socket;

                try
                {
                    // For test
                    // Console.WriteLine("Accepted client");
                    // Console.WriteLine(socket.RemoteEndPoint.AddressFamily.ToString());
                    // End of "For test"

                    NetServer acceptServer = new TCPServer(socket, _port, ServerStates.Accepted);
                    procedure(acceptServer);
                }
                catch(Exception exception)
                {
                    throw exception;
                }
            }
            #endregion

            #region Implementations
            public override void Listen()
            {
                _listeningThread = new Thread(ListenThread);
                _listeningThread.IsBackground = true;
                _listeningThread.Start();
            }

            public override void Close()
            {
                _mainSocket.Close();
                _listeningThread.Abort();
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
                            bytesSent += _mainSocket.Send(bytes, bytesSent, sizeof(int) - bytesSent, SocketFlags.None);
                        }
                        while (bytesSent < sizeof(int));

                        // 2. The compressed flag .... size of bool
                        bytes = System.BitConverter.GetBytes(true);

                        bytesSent = 0;
                        do
                        {
                            bytesSent += _mainSocket.Send(bytes, bytesSent, sizeof(bool) - bytesSent, SocketFlags.None);
                        }
                        while (bytesSent < sizeof(bool));

                        // 3. Data ................... length

                        bytesSent = 0;
                        do
                        {
                            bytesSent += _mainSocket.Send(buffer, bytesSent, buffer.Length - bytesSent, SocketFlags.None);
                        }
                        while (bytesSent < buffer.Length);
                    }
                }
                catch(Exception exception)
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
                        bytesReceived += _mainSocket.Receive(bytes, sizeof(int) - bytesReceived, SocketFlags.None);
                    }
                    while (bytesReceived < sizeof(int));

                    length = System.BitConverter.ToInt32(bytes, 0);


                    // 2. The compressed flag .... size of bool
                    bytesReceived = 0;
                    do
                    {
                        bytesReceived += _mainSocket.Receive(bytes, bytesReceived, sizeof(bool) - bytesReceived, SocketFlags.None);
                    }
                    while (bytesReceived < sizeof(bool));
                    bCompressed = System.BitConverter.ToBoolean(bytes, 0);


                    // 3. Data ................... length
                    bytesReceived = 0;
                    buffer = new byte[length];
                    do
                    {
                        bytesReceived += _mainSocket.Receive(buffer, bytesReceived, length - bytesReceived, SocketFlags.None);
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
                catch(Exception exception)
                {
                    throw exception;
                }
            }         

            #endregion


            #region Properties
            private ServerStates    _state;
            private int             _port;
            private int             _backlog;
            private Socket          _mainSocket;
            private Thread          _listeningThread;
            #endregion
        }
    }

}