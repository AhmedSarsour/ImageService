using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;

namespace ImageServiceCommunication
{
    class Program
    {
        static void Main(string[] args)
        {


                IServer server = new TcpServer(8001, new ClientHandler());

                server.Start();
                server.Stop();
          

        }
    }
}
