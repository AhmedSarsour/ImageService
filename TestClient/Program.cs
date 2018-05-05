using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication.Interfaces;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IClient client = new TcpClientChannel(8001);
            client.Connect();
        }
    }
}
