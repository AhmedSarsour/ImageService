using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Communication.Interfaces;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClientChannel client = new TcpClientChannel(8000);
            client.Connect();
            string str = "";
            Console.WriteLine("Write command: ");

            while ((str = Console.ReadLine()) != "e")
            {

                int c = int.Parse(str[0] + "");

                Console.WriteLine("Result: " + client.sendCommand(c, new string[] { "ab", "cd" }));
                Console.WriteLine("Write command: ");

            }


            client.close();

         
        }
    }
}
