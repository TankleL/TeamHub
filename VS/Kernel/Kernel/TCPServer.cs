using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;


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
                Listening
            }
            #endregion

            #region Constructors
            public TCPServer(string ipAddress, int port, int backlog)
            {
                _port = port;
                _backlog = backlog;
                IPAddress ipAd = IPAddress.Parse(ipAddress);
                _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _mainSocket.Bind(new IPEndPoint(ipAd, _port));

                _state = ServerStates.Idle;
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

                _mainSocket.Listen(_backlog);

                while (true)
                {
                    acceptedSocket = _mainSocket.Accept();

                    acceptThread = new Thread(parStart);
                    acceptThread.Start(acceptedSocket);
                }
            }

            private void AcceptThread(object param)
            {
                Socket socket = param as Socket;

                Console.WriteLine("Accepted client");
                Console.WriteLine(socket.RemoteEndPoint.AddressFamily.ToString());

                _procedure.Invoke(this);
            }
            #endregion

            #region Implementations
            public override void Listen()
            {
                _listeningThread = new Thread(ListenThread);
                _listeningThread.Start();
            }

            public override void Close()
            {
                _listeningThread.Abort();
                _mainSocket.Close();
            }

            public override void Send(NetDataPackage package, Socket socket)
            {

            }

            public override void Receive(out NetDataPackage package, Socket socket)
            {
                int sizeOfPack = 0;

                package = new NetBuffer(sizeOfPack);
            }

            public override void SetProcedure(NetProcedure procedure)
            {
                _procedure = procedure;
            }

            #endregion


            #region Properties
            private ServerStates _state;
            private int _port;
            private int _backlog;
            private Socket _mainSocket;
            private Thread _listeningThread;
            #endregion
        }
    }

}