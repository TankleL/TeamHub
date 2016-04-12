using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public delegate int NetProcedure();
    abstract class NetServer : NetObject
    {
        #region Constructors
        NetServer();
        #endregion

        #region Interfaces
        public virtual void Listen();
        public virtual void Accept();
        public virtual void SetProcedure(NetProcedure procedure);
        #endregion

        #region Properties
        protected NetProcedure _procedure;
        #endregion
    }
}
