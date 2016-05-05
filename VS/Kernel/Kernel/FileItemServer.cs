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
        public class FileItemServer : FileItemObject
        {
            #region Contructors
            public FileItemServer(string path)
            {
                _fileInfo = new FileInfo(path);
            }
            public FileItemServer(FileInfo fi)
            {
                _fileInfo = fi;
            }
            #endregion

            #region Implementations
            public override void SetPath(string path)
            {
                _fileInfo = new FileInfo(path);
            }
            public override string GetPath()
            {
                return _fileInfo.FullName;
            }
            public override DateTime GetCreationTime()
            {
                try
                {
                    return _fileInfo.CreationTime;
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }
            public override DateTime GetLastWriteTime()
            {
                try
                {
                    return _fileInfo.LastWriteTime;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            public override DateTime GetLastAccessTime()
            {
                try
                {
                    return _fileInfo.LastAccessTime;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            public override long GetSize()
            {
                try
                {
                    return _fileInfo.Length;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

            public override void Rename(string name)
            {
                try
                {
                    string destPath = Path.Combine(_fileInfo.DirectoryName, name);
                    _fileInfo.MoveTo(destPath);
                }
                catch (Exception excp)
                {
                    
                    throw excp;
                }
            }
            public override void Delete()
            {
                try
                {
                    _fileInfo.Delete();
                }
                catch (Exception)
                {
                    
                    throw;
                }
                   

            }
            public override void MoveTo(string dest_path)
            {
                try
                {
                    _fileInfo.MoveTo(dest_path);
                }
                catch (Exception)
                {
                    
                    throw;
                }

            }
            public override void CopyTo(string dest_path)
            {
                try
                {
                    _fileInfo.CopyTo(dest_path, true);
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            #endregion

            #region Properties
            private FileInfo _fileInfo;
            #endregion

        }
    }
}
