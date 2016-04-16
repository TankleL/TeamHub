using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamHub.Kernel;

namespace Client_Test
{
    

    class Program
    {
        static void Main(string[] args)
        {
            TCPClient client = new TCPClient("127.0.0.1", 5628);
            client.Connect();

            string text = Console.ReadLine();
            NetBuffer buffer = new NetBuffer(2048);
            buffer.Write(text);

            client.Send(buffer);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);

            client.Close();
        }
    }
}
