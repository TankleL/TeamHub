using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TeamHub.Kernel;

namespace Kernel_Test
{
    //class NetProcedure
    //{
    //    public void Proc(NetObject obj)
    //    {
    //        TCPServer server = obj as TCPServer;

    //        NetDataPackage pack;
    //        server.Receive(out pack);

    //        ProcessOperation(pack, server);
    //        /*
    //        NetBuffer feedback = DiskNodeItem.PackOperator.PackOperationInfo(DiskNodeItem.Operation.SUCCESS, 0, "", "");
    //        server.Send(feedback);*/
    //        Console.Write("Yse!");
    //    }

    //    public void ProcessOperation(NetDataPackage pack, TCPServer server)
    //    {
    //        DiskNodeItem.Operation op;
    //        string dest_path;
    //        DiskNodeItem node_server;
    //        try 
    //        {	        
    //            DiskNodeItem.PackOperator.UnpackOperationInfo(pack, out op, out node_server, out dest_path);
    //            switch(op)
    //            {
    //                case DiskNodeItem.Operation.COPYTO:                        
    //                    node_server.CopyTo(dest_path);
    //                    break;
    //                case DiskNodeItem.Operation.DELETE:
    //                    node_server.Delete();
    //                    break;
    //                case DiskNodeItem.Operation.MOVETO:
    //                    node_server.MoveTo(dest_path);
    //                    break;
    //                case DiskNodeItem.Operation.RENAME:
    //                    node_server.Rename(dest_path);
    //                    break;
    //                case DiskNodeItem.Operation.CREATESUBDIRTORY:
    //                    DirItemServer dir = node_server as DirItemServer;
    //                    dir.CreateSubDirectory(dest_path);
    //                    break;
    //                case DiskNodeItem.Operation.GETSUBITEMS:
    //                    DirItemServer dir2 = node_server as DirItemServer;
    //                    DiskNodeItem[] items = dir2.GetSubItems();
    //                    NetBuffer package = DiskNodeItem.PackOperator.PackSubItems(DiskNodeItem.Operation.GETSUBITEMS, node_server.GetPath(), items);
    //                    server.Send(package);
    //                    break;
    //            }            

    //        }
    //        catch (Exception excp)
    //        {

    //            Console.WriteLine(excp.Message);
    //        }

    //    }

    //}


    //class Program
    //{

    //    static void Main(string[] args)
    //    {
    //        NetProcedure netProc = new NetProcedure();

    //        TCPServer server = new TCPServer(5628, 20);
    //        server.procedure = new NetServer.Procedure(netProc.Proc);
    //        server.Listen();

    //        Console.WriteLine("Press any key to continue...");
    //        Console.ReadKey(true);

    //        server.Close();
    //    }
    //}

    class NetProcedure
    {
        public void Proc(NetObject obj)
        {
            TCPServer server = obj as TCPServer;

            Console.WriteLine("Into Proc");

            NetDataPackage pack;
            server.Receive(out pack);

            string text = String.Empty;
            pack.Read(out text, pack.StringLength());

            Console.WriteLine("From Client:\n" + text);
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
