﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamHub
{
    namespace Kernel
    {
        public abstract class FileItemObject : DiskNodeItem
        {
            #region Interfaces

            #endregion
            #region Implementations
            public override DiskNodeType GetNodeType()
            {
                return DiskNodeType.FILE;
            }
            #endregion

        }
    }
}
