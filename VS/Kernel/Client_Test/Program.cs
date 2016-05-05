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

            try
            {
                DateTime time = new DateTime(2000,1,2);
                DirItemClient dir = new DirItemClient(@"E:\Test", time, time, time, 100, client);
                dir.Rename("Test1");
                

            }
            catch (Exception excp)
            {
                Console.WriteLine(excp.Message);
            }


            

            Console.ReadKey(true);

            client.Close();
        }
    }
}
