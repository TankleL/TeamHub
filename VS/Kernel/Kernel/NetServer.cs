using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TeamHub
{

    namespace Kernel
    {
        public abstract class NetServer : NetObject
        {
            #region Delegates
            public delegate void Procedure(NetObject obj);
            #endregion

            #region Constructors
            public NetServer()
            { }
            #endregion

            #region Interfaces
            public abstract void Listen();
            public abstract void Close();
            #endregion

            #region Properties
            public Procedure procedure;
            #endregion
        }
    }
}