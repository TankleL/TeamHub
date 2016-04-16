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
