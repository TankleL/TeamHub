using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    abstract class NetObject
    {
        #region Constructors
        NetObject();
        
        #endregion
        

        #region Interfaces
        public virtual void Send();
        public virtual void Receive();
        #endregion
    }
}
