using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamHub
{
    namespace Kernel
    {
        class FileItemClient : FileItemObject
        {
            public FileItemClient(string path, DateTime crttime, DateTime lwtime, DateTime latime, long nodesize, TCPClient tcpClient = null)
            {
                _nodePath = path;
                _creationTime = crttime;
                _lastWriteTime = lwtime;
                _lastAccessTime = latime;
                _nodeSize = nodesize;
                _nodeType = DiskNodeType.FILE;
                SetClient(tcpClient);
            }

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
