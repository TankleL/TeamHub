using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public abstract void Accept();
        public abstract void SetProcedure(NetProcedure procedure);
        #endregion

        #region Properties
        protected NetProcedure _procedure;
        #endregion
    }
}
