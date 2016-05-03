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
            #region Enumerations
            public enum Operation
            {
                RENAME = 0,
                DELETE,
                MOVETO,
                COPYTO,
                OPEN,
                GETSUBITEMS,
                CREATESUBDIRTORY
            }
            #endregion
            #region Interfaces
            public DiskNodeItem[] GetSubItems();
            public void CreateSubDirectory(string dir_name);
            #endregion
        }
    }
}
