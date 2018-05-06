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
            Console.WriteLine("Result: " + client.sendCommand((int)CommandEnum.GetConfigCommand, new string[] { "ab", "cd" }));

            Console.WriteLine("Result2: " + client.sendCommand((int)CommandEnum.GetConfigCommand, new string[] { "ab", "cd" }));

            client.close();

         
        }
    }
}
