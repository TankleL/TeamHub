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
        public class FileItemServer : DiskNodeItem
        {
            #region Contructors
            public FileItemServer(string path,DateTime crttime, DateTime lwtime, DateTime latime, long nodesize, bool islocal)
            {
                SetMembers(path, crttime, lwtime, latime, nodesize, islocal);
                NodeType        = DiskNodeType.FILE;

            }
            public FileItemServer(FileInfo fi, bool islocal)
            {
                NodePath = fi.FullName;
                CreationTime = fi.CreationTime;
                LastWriteTime = fi.LastWriteTime;
                LastAccessTime = fi.LastAccessTime;
                NodeType = DiskNodeType.FILE;
                NodeSize = fi.Length;
                IsLocal = islocal;                
            }
            #endregion

            #region OverrideAbstractMethod
            public override void Rename(string name)
            {
                if(IsLocal)
                {

                }
                else
                {
                    FileInfo fi = new FileInfo(NodePath);
                    fi.MoveTo(Path.Combine(fi.Directory.ToString(), name));
                }
                
            }
            public override void Delete()
            {
                if(IsLocal)
                { }
                else
                { 
                    File.Delete(NodePath);
                }
            }
            public override void MoveTo(string dest_path)
            {
                if(IsLocal)
                { }
                else
                {
                    File.Move(NodePath, dest_path);
                }    

            }
            public override void CopyTo(string dest_path)
            {
                File.Copy(NodePath, dest_path, true);
            }
            #endregion

            // ---------------- 增加的成员方法 ---------------------

        }
    }
}
