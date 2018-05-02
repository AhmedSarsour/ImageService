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
            this.port = port;

        }

        public void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            Console.WriteLine("You are connected");
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                Console.Write("Please enter a message: ");
                string msg = Console.ReadLine();
                writer.Write(msg);
                // Get result from server
                string result = reader.ReadString();
                Console.WriteLine("Result = {0}", result);
            }
            client.Close();
        }
    }
}
