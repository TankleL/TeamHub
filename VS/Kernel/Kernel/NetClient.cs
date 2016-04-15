using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeamHub
{

    namespace Kernel
    {
        public abstract class NetClient : NetObject
        {
            #region Constructors
            public NetClient()
            { }
            #endregion

            #region Interfaces
            public abstract void connect();
            #endregion
        }
    }
}