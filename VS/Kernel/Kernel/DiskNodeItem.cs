using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TeamHub
{
    namespace Kernel
    {
        /// <summary>
        /// 异常参见 DirectoryInfo and FileInfo class
        /// </summary>   
        public abstract class DiskNodeItem
        {
            #region Enumerations
            public enum DiskNodeType
            {
                FILE = 0,
                DIRECTORY
            }
            #endregion

            #region Interfaces
            public abstract void SetPath(string path);
            public abstract string GetPath();
            public abstract DiskNodeType GetNodeType();
            public abstract DateTime GetCreationTime();
            public abstract DateTime GetLastWriteTime();
            public abstract DateTime GetLastAccessTime();
            public abstract long GetSize();

            public abstract void Rename(string name);
            public abstract void Delete();
            public abstract void MoveTo(string dest_path);
            public abstract void CopyTo(string dest_path);

            #endregion         


        }
    }
}
