using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamHub
{
    namespace Kernel
    {
        public abstract class DirItemObject: DiskNodeItem
        {

            #region Interfaces
            public abstract DiskNodeItem[] GetSubItems();
            public abstract void CreateSubDirectory(string dir_name);
            #endregion

            #region Implementations
            public override DiskNodeType GetNodeType()
            {
                return DiskNodeType.DIRECTORY;
            }
            #endregion

        }
    }
}
