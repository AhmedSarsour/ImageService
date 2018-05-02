using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace ImageServiceCommunication
{
    public class TcpClientChannel:IClient
    {
        private int port;
        public TcpClientChannel(int port)
        {
            this.port = port;;
        }

        public void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            Console.WriteLine("You are connected");
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Send data to server
                Console.Write("Please enter a number: ");
                int num = int.Parse(Console.ReadLine());
                writer.Write(num);
                // Get result from server
                int result = reader.Read();
                Console.WriteLine("Result = {0}", result);
            }
            client.Close();
        }
    }
}
