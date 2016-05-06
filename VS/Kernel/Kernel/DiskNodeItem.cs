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
            public enum Operation
            {
                RENAME = 0,
                DELETE,
                MOVETO,
                COPYTO,
                GETSUBITEMS,
                CREATESUBDIRTORY,
                GETITEMINFO,
                SUCCESS,
                ERROR
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


            #region StaticClass
            public static class PackOperator
            {
                public const int BufferMaxSize = 4096;

                // package的读指针不变
                public static Operation GetOperation(NetDataPackage package)
                {
                    int operation;
                    NetBuffer packBuffer = package as NetBuffer;
                    uint pointer = packBuffer.GetReadPointer();
                    try
                    {
                        packBuffer.Read(out operation);
                        packBuffer.Seek_ReadPointerBegin(pointer);
                        return (Operation)operation;
                    }
                    catch (Exception excp)
                    {

                        throw excp;
                    }
                }
                /// <summary>
                ///     Formate:
                ///     --------------------- CommPack ---------------
                ///     |   1.The operation ............... int       |
                ///     |   2.The type of node ............ int       |
                ///     |   3.The length of path .......... int       |
                ///     |   4.The Path .................... string    |
                ///     |   5.The length of destiny path .. int       |
                ///     |   6.The destiny path ............ string    |
                ///     ----------------------------------------------
                /// </summary>
                public static NetBuffer PackOperationInfo(Operation op, DiskNodeType node_type, string src_path, string dest_path)
                {
                    try
                    {
                        int packageSize = sizeof(int) * 4 + (src_path.Length + dest_path.Length) * 2;
                        NetBuffer package = new NetBuffer(packageSize);

                        package.Write((int)op);

                        package.Write((int)node_type);

                        package.Write(src_path.Length);
                        package.Write(src_path);

                        package.Write(dest_path.Length);
                        package.Write(dest_path);

                        return package;
                    }
                    catch (Exception excp)
                    {
                        throw excp;
                    }
                }
                public static NetBuffer PackOperationInfo(Operation op, DiskNodeItem src_node, string dest_path)
                {

                    try
                    {
                        return PackOperationInfo(op, src_node.GetNodeType(), src_node.GetPath(), dest_path);
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                }
                public static void UnpackOperationInfo(NetDataPackage package, out Operation op, out DiskNodeType node_type, out string src_path, out string dest_path)
                {
                    int operation;
                    int pathLength;
                    try
                    {
                        package.Read(out operation);
                        op = (Operation)operation;

                        package.Read(out operation);
                        node_type = (DiskNodeType)operation;

                        package.Read(out pathLength);
                        package.Read(out src_path, (uint)pathLength);

                        package.Read(out pathLength);
                        package.Read(out dest_path, (uint)pathLength);
                    }
                    catch (Exception excp)
                    {
                        throw excp;
                    }
                }
                public static void UnpackOperationInfo(NetDataPackage package, out Operation op,out DiskNodeItem src_node, out string dest_path)
                {
                    int operation;
                    int pathLength;
                    DiskNodeType node_type;
                    string src_path;
                    try
                    {
                        package.Read(out operation);
                        op = (Operation)operation;

                        package.Read(out operation);
                        node_type = (DiskNodeType)operation;

                        package.Read(out pathLength);
                        package.Read(out src_path, (uint)pathLength);

                        package.Read(out pathLength);
                        package.Read(out dest_path, (uint)pathLength);

                        switch (node_type)
                        {
                            case DiskNodeType.DIRECTORY:
                                src_node = new DirItemServer(src_path);
                                break;
                            case DiskNodeType.FILE:
                                src_node = new FileItemServer(src_path);
                                break;
                            default:
                                throw new Exception("DiskNodeType Error");
                        }

                    }
                    catch (Exception excp)
                    {
                        throw excp;
                    }
                }

                /// <summary>
                ///     --------------------- CommPack ---------------
                ///     |   1.The operation ............... int       |
                ///     |   2.The type of node ............ int       |
                ///     |   3.The length of path .......... int       |
                ///     |   4.The Path .................... string    |
                ///     |   5.The CreationTime ............ long      |
                ///     |   6.The LastWriteTime ........... long      |
                ///     |   7.The LastAccessTime .......... long      |
                ///     |   8.The size of node ............ long      |
                ///     ----------------------------------------------
                /// </summary>
                /// <returns></returns>
                public static NetBuffer PackItemInfo(DiskNodeItem node_server)
                {
                    int operation;
                    int pathLength;
                    string path;
                    long creationTime, lastWriteTime, lastAccessTime;
                    int nodeType;
                    long nodeSize;

                    int longSize = sizeof(long);
                    byte[] bits = new byte[longSize];

                    NetBuffer package;
                    try
                    {
                        operation = (int)Operation.GETITEMINFO;
                        path = node_server.GetPath();
                        pathLength = path.Length;
                        creationTime = node_server.GetCreationTime().ToBinary();
                        lastWriteTime = node_server.GetLastWriteTime().ToBinary();
                        lastAccessTime = node_server.GetLastAccessTime().ToBinary();
                        nodeType = (int)node_server.GetNodeType();
                        nodeSize = node_server.GetSize();

                        int packageLength = sizeof(int) * 3 + pathLength * 2 + sizeof(long) * 4;
                        package = new NetBuffer(packageLength);

                        package.Write(operation);

                        package.Write((int)node_server.GetNodeType());

                        package.Write(pathLength);
                        package.Write(path);

                        bits = System.BitConverter.GetBytes(creationTime);
                        package.Write(bits);

                        bits = System.BitConverter.GetBytes(lastWriteTime);
                        package.Write(bits);

                        bits = System.BitConverter.GetBytes(lastAccessTime);
                        package.Write(bits);

                        bits = System.BitConverter.GetBytes(nodeSize);
                        package.Write(bits);

                        return package;

                    }
                    catch (Exception excp)
                    {

                        throw excp;
                    }

                }
                public static void UnpackItemInfo(NetDataPackage package, out Operation op, out DiskNodeType node_type ,out string path, out DateTime creation_time, out DateTime lastwrite_time, out DateTime lastaccess_time, out long node_size)
                {
                    int intTemp;
                    int longSize = sizeof(long);
                    byte[] bits = new byte[longSize];

                    long longTemp;
                    try
                    {
                        //1.The operation ............... int 
                        package.Read(out intTemp);
                        op = (Operation)intTemp;

                        // 2.The Type of node ............ int  
                        package.Read(out intTemp);
                        node_type = (DiskNodeType)intTemp;

                        // 3.The length of path .......... int           
                        // 4.The Path .................... string    
                        package.Read(out intTemp);
                        package.Read(out path, (uint)intTemp);

                        // 5.The CreationTime ............ long                                  
                        package.Read(out bits, (uint)longSize);
                        longTemp = System.BitConverter.ToInt64(bits, 0);
                        creation_time = DateTime.FromBinary(longTemp);

                        // 6.The LastWriteTime ........... long  
                        package.Read(out bits, (uint)longSize);
                        longTemp = System.BitConverter.ToInt64(bits, 0);
                        lastwrite_time = DateTime.FromBinary(longTemp);

                        // 7.The LastAccessTime .......... long      
                        package.Read(out bits, (uint)longSize);
                        longTemp = System.BitConverter.ToInt64(bits, 0);
                        lastaccess_time = DateTime.FromBinary(longTemp);

                        // 8.The size of node ............ long  
                        package.Read(out bits, (uint)longSize);
                        node_size = System.BitConverter.ToInt64(bits, 0);
                    }
                    catch (Exception excp)
                    {

                        throw excp;
                    }

                }
                public static void UnpackItemInfo(NetDataPackage package, out Operation op, out DiskNodeItem item)
                {
                    string path;
                    DateTime creation_time;
                    DateTime lastwrite_time;
                    DateTime lastaccess_time;
                    DiskNodeType node_type;
                    long node_size;
                    UnpackItemInfo(package, out op, out node_type, out path, out creation_time, out lastwrite_time, out lastaccess_time, out node_size);
                    if (node_type == DiskNodeType.DIRECTORY)
                        item = new DirItemClient(path, creation_time, lastwrite_time, lastaccess_time, node_size);
                    else
                        item = new FileItemClient(path, creation_time, lastwrite_time, lastaccess_time, node_size);

                }
                /// <summary>
                ///     Formate:
                ///     --------------------- CommPack ---------------
                ///     |   1.The operation ............... int       |
                ///     |   2.The type of node ............ int       |
                ///     |   3.The length of path .......... int       |
                ///     |   4.The Path .................... string    |
                ///     |   5.The number of SubItems ... .. int       |
                ///     |   6.The SubItem ................. ServerInfo|
                ///     |   7.(5)循环 number - 1 次                   |
                ///     ----------------------------------------------
                /// </summary>
                /// <returns></returns>
                public static NetBuffer PackSubItems(Operation op, string path, DiskNodeItem[] subitems)
                {
                    int operation;
                    NetBuffer package;

                    try
                    {
                        // 获取subitem长度
                        byte[][] tempBuffer = new byte[subitems.Length][];
                        int i = 0;
                        int packageSize = sizeof(int) * 4 + path.Length * 2;
                        foreach (DiskNodeItem dni in subitems)
                        {
                            PackItemInfo(dni).Buffer(out tempBuffer[i++]);
                            packageSize += tempBuffer[i - 1].Length;
                        }

                        package = new NetBuffer(packageSize);
                        operation = (int)op;
                        package.Write(operation);

                        package.Write((int)DiskNodeType.DIRECTORY);

                        package.Write(path.Length);
                        package.Write(path);

                        package.Write(subitems.Length);

                        foreach(byte[] item in tempBuffer)
                        {
                            package.Write(item);
                        }

                        return package;
                    }
                    catch (Exception excp)
                    {

                        throw excp;
                    }
                }
                public static void UnpackSubItems(NetDataPackage package, out Operation op, out string path, out DiskNodeItem[] subitems)
                {
                    int intTemp;
                    Operation opTemp;
                    try
                    {
                        package.Read(out intTemp);
                        op = (Operation)intTemp;

                        // 2.The type of node ............ int 
                        package.Read(out intTemp);

                        package.Read(out intTemp);
                        package.Read(out path, (uint)intTemp);

                        if (op != Operation.GETSUBITEMS && op != Operation.SUCCESS)
                        {
                            subitems = null;
                            return;
                        }
                        package.Read(out intTemp);
                        subitems = new DiskNodeItem[intTemp];

                        DiskNodeItem tempNode;
                        for (int i = 0; i < intTemp; i++)
                        {
                            UnpackItemInfo(package, out opTemp, out tempNode);
                            subitems[i] = tempNode;
                        }
                    }
                    catch (Exception excp)
                    {

                        throw excp;
                    }
                }

            }
            #endregion

        }
    }
}
