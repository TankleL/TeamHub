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
        public class DirItemServer : DirItemObject
        {
            #region Constructors
            public DirItemServer(DirectoryInfo di)
            {
                _dirInfo = di;
            }
            public DirItemServer(String path)
            {
                _dirInfo = new DirectoryInfo(path);
            }

            // 解析包并执行包内操作
            public DirItemServer(NetDataPackage pack)
            {
                ParsePack(pack);
            }
            #endregion

            #region Implementations
            public override void SetPath(string path)
            {
                _dirInfo = new DirectoryInfo(path);
            }
            public override string GetPath()
            {
                try
                {
                    return _dirInfo.FullName;
                }
                catch(Exception excp)
                {
                    throw excp;
                }
            }
            public override DiskNodeType GetNodeType()
            {
                return DiskNodeType.DIRECTORY;
            }
            public override DateTime GetCreationTime()
            {
                try
                {
                    return _dirInfo.CreationTime;
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }
            public override DateTime GetLastWriteTime()
            {
                try
                {
                    return _dirInfo.LastWriteTime;
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }
            public override DateTime GetLastAccessTime()
            {
                try
                {
                    return _dirInfo.LastAccessTime;
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }
            public override long GetSize()
            {
                try
                {
                    return _getDirectorySize(_dirInfo);
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }

            public override void Rename(string name)
            {
                try 
                {
                    DirectoryInfo thisDir = _dirInfo;
                    DirectoryInfo parentDirInfo = thisDir.Parent;
                    string destPath = Path.Combine(parentDirInfo.ToString(), thisDir.Name);
                    thisDir.MoveTo(destPath);
                }
                catch(Exception excp)
                {
                    throw excp;
                }

            }
            public override void Delete()
            {
                try
                { 
                    _dirInfo.Delete(true);
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }
            public override void MoveTo(string dest_path)
            {
                try
                {
                    _dirInfo.MoveTo(dest_path);
                }
                catch(Exception excp)
                {
                    throw excp;
                }


            }
            public override void CopyTo(string dest_path)
            {
                try
                {
                    DirectoryInfo dest = new DirectoryInfo(dest_path);
                    _copyAll(_dirInfo, dest);

                }
                catch (Exception excp)
                {
                    throw excp;
                }
                
            }
            public DiskNodeItem[] GetSubItems()
            {
                try
                {
                    List<DiskNodeItem> dItems = new List<DiskNodeItem>();
                    DirectoryInfo thisNode = _dirInfo;
                    foreach (FileInfo fi in thisNode.GetFiles())
                    {
                        dItems.Add(new FileItemServer(fi));
                    }
                    foreach (DirectoryInfo di in thisNode.GetDirectories())
                    {
                        dItems.Add(new DirItemServer(di));
                    }
                    return dItems.ToArray();
                }
                catch (Exception excp)
                {
                    throw excp;
                }

            }
            public void CreateSubDirectory(string dir_name)
            {
                try
                {
                    _dirInfo.CreateSubdirectory(dir_name);
                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }
            #endregion

            #region Methods
            public void ParsePack(NetDataPackage pack)
            {
                int operation;
                int pathLength;
                string path;
                int destPathLength;
                string destPath;
                try
                {
                    pack.Read(out operation);

                    pack.Read(out pathLength);
                    pack.Read(out path, (uint)pathLength);

                    _dirInfo = new DirectoryInfo(path);

                    switch ((Operation)operation)
                    {
                        case Operation.COPYTO:
                            pack.Read(out destPathLength);
                            pack.Read(out destPath, (uint)destPathLength);
                            CopyTo(destPath);
                            break;
                        case Operation.DELETE:
                            Delete();
                            break;
                        case Operation.MOVETO:
                            pack.Read(out destPathLength);
                            pack.Read(out destPath, (uint)destPathLength);
                            MoveTo(destPath);
                            break;
                        case Operation.OPEN:
                            break;
                        case Operation.RENAME:
                            pack.Read(out destPathLength);
                            pack.Read(out destPath, (uint)destPathLength);
                            Rename(destPath);
                            break;
                        case Operation.CREATESUBDIRTORY:
                            pack.Read(out destPathLength);
                            pack.Read(out destPath, (uint)destPathLength);
                            CreateSubDirectory(destPath);
                            break;
                        case Operation.GETSUBITEMS:
                            NetBuffer sendPack = (NetBuffer)_getSubItemsPack();

                            break;
                    }

                }
                catch (Exception excp)
                {
                    throw excp;
                }
            }

            #endregion

            #region PrivateMethods
            private void _copyAll(DirectoryInfo source, DirectoryInfo target)
            {

                if (source.FullName.ToLower() == target.FullName.ToLower())
                    return;

                // Check if the destination directory exists, if not, create it. 
                target.Create();

                // Copy each file into it's new directory. 
                foreach (FileInfo fi in source.GetFiles())
                {
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                }

                // Copy each subdirectory using recursion. 
                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    _copyAll(diSourceSubDir, nextTargetSubDir);
                }

            }
            private long _getDirectorySize(DirectoryInfo target)
            {
                long size = 0;
                // Plus the length of each file 
                foreach (FileInfo fi in target.GetFiles())
                {
                    size += fi.Length;
                }

                // Plus size of each subdirectory
                foreach (DirectoryInfo di in target.GetDirectories())
                {
                    size += _getDirectorySize(di);
                }
                return size;
            }

            private NetDataPackage _getSubItemsPack()
            {

            }
            #endregion

            #region Properties
            private DirectoryInfo _dirInfo;
            #endregion

        }
    }
}
