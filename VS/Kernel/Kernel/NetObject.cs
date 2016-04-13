﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel
{
    public abstract class NetObject
    {
        #region Constructors
        public NetObject()
        { }
        
        #endregion
        

        #region Interfaces
        public abstract void Send();
        public abstract void Receive();
        #endregion
    }
}
