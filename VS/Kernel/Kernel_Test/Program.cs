using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TeamHub.Kernel;

namespace Kernel_Test
{
    class NetProcedure
    {
        public void Proc(NetObject obj)
        {
            TCPServer server = obj as TCPServer;

            NetDataPackage pack;
            server.Receive(out pack);
            DiskNodeItem.Operation status;
            string path;
            string dest_path;
            DiskNodeItem.PackOperator.UnpackOperationInfo(pack,out status,out path,out dest_path);

            Console.WriteLine(path + dest_path);
            NetBuffer pack1 = DiskNodeItem.PackOperator.PackOperationInfo(DiskNodeItem.Operation.SUCCESS, "", "");
            server.Send(pack1);

        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            NetProcedure netProc = new NetProcedure();

            TCPServer server = new TCPServer(5628, 20);
            server.procedure = new NetServer.Procedure(netProc.Proc);
            server.Listen();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            server.Close();
        }
    }
}
