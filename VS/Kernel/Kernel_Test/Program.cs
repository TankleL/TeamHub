using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TeamHub.Kernel;

namespace Kernel_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //TCPServer server = new TCPServer("127.0.0.1", 5628, 20);
            //server.Listen();

            NetBuffer nbuf = new NetBuffer(20480);
            StreamReader rdr = new StreamReader("text.txt");

            string origin = rdr.ReadToEnd();
            rdr.Close();

            Console.WriteLine("origin:");
            Console.WriteLine(origin);
            nbuf.Write(origin);

            byte[] bufferShrinked;
            byte[] bufferUnShrinked;
            nbuf.Shrink(out bufferShrinked, true);
            nbuf.Buffer(out bufferUnShrinked);

            string result;
            nbuf.Read(out result, nbuf.Count());

            Console.WriteLine("Result:");
            Console.WriteLine(result);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
