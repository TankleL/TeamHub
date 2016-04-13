using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    abstract public class NetObject
    {
        #region Constructors
        public NetObject()
        { }
        
        #endregion
        

        #region Interfaces
        public virtual void Send()
        { }
        public virtual void Receive()
        { }
        #endregion
    }
}
