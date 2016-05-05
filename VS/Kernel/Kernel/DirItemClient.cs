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
                SetClient(tcpClient);
            }
            
            #endregion

            #region Implementations

            public override void SetPath(string path)
            {
                _nodePath = path;
            }
            public override string GetPath()
            {
                return _nodePath;
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
        
            public override void Rename(string name)
            {
                try
                {
                    NetBuffer sendPack = PackOperator.PackOperationInfo(Operation.RENAME, _nodePath, name);
                    _client.Send(sendPack);
                    
                    // wait for responding from server
                    NetDataPackage receivePackage;
                    _client.Receive(out receivePackage);

                    FeedbackProcessing(receivePackage);

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
                    NetBuffer pack = PackOperator.PackOperationInfo(Operation.DELETE, _nodePath, "");
                    _client.Send(pack);

                    // wait for responding from server
                    NetDataPackage receivePackage;
                    _client.Receive(out receivePackage);

                    FeedbackProcessing(receivePackage);

                }
                catch(Exception excp)
                {
                    throw excp;
                }
            }
           
            public override void MoveTo(string dest_path)
            {
                try
                {
                    NetBuffer pack = PackOperator.PackOperationInfo(Operation.MOVETO, _nodePath, dest_path);
                    _client.Send(pack);

                    // wait for responding from server
                    NetDataPackage receivePackage;
                    _client.Receive(out receivePackage);

                    FeedbackProcessing(receivePackage);

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
                    NetBuffer pack = PackOperator.PackOperationInfo(Operation.COPYTO,_nodePath, dest_path);
                    _client.Send(pack);

                    // wait for responding from server
                    NetDataPackage receivePackage;
                    _client.Receive(out receivePackage);

                    FeedbackProcessing(receivePackage);
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
            private void FeedbackProcessing(NetDataPackage receivePackage)
            {
                Operation status;
                string message1;
                string message2;
                PackOperator.UnpackOperationInfo(receivePackage, out status, out message1, out message2);
                if (status == Operation.SUCCESS)
                    return;
                else if (status == Operation.ERROR)
                {
                    Exception error = new Exception(message1 + "," + message2);
                    throw error;
                }
                else
                {
                    Exception error = new Exception("服务器回应请求错误");
                    throw error;
                }
            }
            #endregion

            #region Properties
            private string _nodePath;           // 当前节点的绝对路径
            private DateTime _creationTime;     // 节点 创建时间
            private DateTime _lastWriteTime;    // 节点 最后一次修改时间
            private DateTime _lastAccessTime;   // 节点 最后一次读取时间
            private long _nodeSize;             // 节点 大小（Byte）
            private TCPClient _client;
            #endregion

        }
    }
}
