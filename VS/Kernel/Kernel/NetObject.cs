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
        public abstract class NetObject
        {
            #region Constructors
            public NetObject()
            { }

            #endregion


            #region Interfaces
            public abstract void Send(NetDataPackage package, Socket socket);
            public abstract void Receive(out NetDataPackage package, Socket socket);
            #endregion
        }
    }
}