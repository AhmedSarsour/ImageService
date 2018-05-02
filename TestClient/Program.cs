using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;
using ImageServiceCommunication;
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
