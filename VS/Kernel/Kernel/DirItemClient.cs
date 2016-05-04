using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamHub
{
    namespace Kernel
    {
        public class DirItemClient : DirItemObject
        {
            #region Constructors
            public DirItemClient(string path, DateTime crttime, DateTime lwtime, DateTime latime, long nodesize, TCPClient tcpClient = null)
            {
                _nodePath = path;
                _creationTime = crttime;
                _lastWriteTime = lwtime;
                _lastAccessTime = latime;
                _nodeSize = nodesize;
                _nodeType = DiskNodeType.DIRECTORY;
                SetClient(tcpClient);
            }
            
            #endregion

            #region Implementations

            /// <summary>
            ///     Formate:
            ///     --------------------- CommPack --------------
            ///     |   1.The operation ............. int       |
            ///     |   2.The length of path ........ int       |
            ///     |   3.The Path .................. string    |
            ///     |   4.The length of name ........ int (0)   |
            ///     |   5.The new name .............. string    |
            ///     ---------------------------------------------
            /// </summary>
            public override void SetPath(string path)
            {
                _nodePath = path;
            }
            public override string GetPath()
            {
                return _nodePath;
            }
            public override DiskNodeType GetNodeType()
            {
                return _nodeType;
            }
            public override DateTime GetCreationTime()
            {
                return _creationTime;
            }
            public override DateTime GetLastWriteTime()
            {
                return _lastWriteTime;
            }
            public override DateTime GetLastAccessTime()
            {
                return _lastAccessTime;
            }
            public override long GetSize()
            {
                return _nodeSize;
            }

            /// <summary>
            ///     Formate:
            ///     --------------------- CommPack --------------
            ///     |   1.The operation ............. int       |
            ///     |   2.The length of path ........ int       |
            ///     |   3.The Path .................. string    |
            ///     |   4.The length of name ........ int       |
            ///     |   5.The new name .............. string    |
            ///     ---------------------------------------------
            /// </summary>
            public override void Rename(string name)
            {
                try
                {
                    NetBuffer sendPack = PackOperator.PackClientInfo(Operation.RENAME, _nodePath, name);
                    _client.Send(sendPack);
                    
                    // wait for responding from server
                    NetDataPackage receivePack;
                    _client.Receive(out receivePack);




                }
                catch(Exception excp)
                {
                    throw excp;
                }
            }
            /// <summary>
            ///     Formate:
            ///     --------------------- CommPack ---------------
            ///     |   1.The operation ............... int       |
            ///     |   2.The length of path .......... int       |
            ///     |   3.The Path .................... string    |
            ///     |   4.The length of destiny path .. int (0)   |
            ///     |   5.The destiny path ............ string    |
            ///     ----------------------------------------------
            /// </summary>
            public override void Delete()
            {
                try
                {
                    NetBuffer pack = PackOperator.PackClientInfo(Operation.DELETE, _nodePath, "");
                    _client.Send(pack);
                }
                catch(Exception excp)
                {
                    throw excp;
                }
            }

            /// <summary>
            ///     Formate:
            ///     --------------------- CommPack ---------------
            ///     |   1.The operation ............... int       |
            ///     |   2.The length of path .......... int       |
            ///     |   3.The Path .................... string    |
            ///     |   4.The length of destiny path .. int       |
            ///     |   5.The destiny path ............ string    |
            ///     ----------------------------------------------
            /// </summary>
            public override void MoveTo(string dest_path)
            {
                try
                {
                    NetBuffer pack = new NetBuffer(2048);

                    pack.Write((int)Operation.MOVETO);

                    pack.Write(_nodePath.Length);
                    pack.Write(_nodePath);

                    pack.Write(dest_path.Length);
                    pack.Write(dest_path);

                    _client.Connect();
                    _client.Send(pack);

                    _client.Close();
                }
                catch (Exception excp)
                {
                    throw excp;
                }         
            }
           
            public override void CopyTo(string dest_path)
            {
                try
                {
                    NetBuffer pack = new NetBuffer(2048);

                    pack.Write((int)Operation.COPYTO);

                    pack.Write(_nodePath.Length);
                    pack.Write(_nodePath);

                    pack.Write(dest_path.Length);
                    pack.Write(dest_path);

                    _client.Connect();
                    _client.Send(pack);

                    _client.Close();
                }
                catch (Exception excp)
                {
                    throw excp;
                }                   
            }


            #endregion

            #region Methods
            public void SetClient(TCPClient tcpClient)
            {
                _client = tcpClient;
            }
            #endregion

            #region Properties
            private string _nodePath;           // 当前节点的绝对路径
            private DateTime _creationTime;     // 节点 创建时间
            private DateTime _lastWriteTime;    // 节点 最后一次修改时间
            private DateTime _lastAccessTime;   // 节点 最后一次读取时间
            private DiskNodeType _nodeType;     // 节点 类型
            private long _nodeSize;             // 节点 大小（Byte）

            private TCPClient _client;
            #endregion

        }
    }
}
