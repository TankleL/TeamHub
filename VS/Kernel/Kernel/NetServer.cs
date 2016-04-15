using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeamHub
{

    namespace Kernel
    {
        public delegate int NetProcedure(NetServer server);
        public abstract class NetServer : NetObject
        {
            #region Constructors
            public NetServer()
            { }
            #endregion

            #region Interfaces
            public abstract void Listen();
            public abstract void SetProcedure(NetProcedure procedure);
            public abstract void Close();
            #endregion

            #region Properties
            protected NetProcedure _procedure;
            #endregion
        }
    }
}