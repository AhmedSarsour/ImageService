using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace ImageService.Communication
{
    public class TcpClientChannel
    {
        private int port;
        private TcpClient client;
        private NetworkStream stream = null;
        private BinaryReader reader;
        private BinaryWriter writer;
        public TcpClientChannel(int port)
        {
            this.port = port;
    
        }

        public void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            client = new TcpClient();
            client.Connect(ep);

            Console.WriteLine("You are connected");
        }

        public string sendCommand(int id, string []args)
        {
            if (stream == null)
            {
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                writer = new BinaryWriter(stream);
            }
            using (stream)
            using (reader)
            using (writer)
            {
                // Send data to server
                Console.WriteLine("Sending the command with id " + id);
                //We will send the commend seperated by # chars. the first char will be the id of the command.
                string send = id + "";
                if (args != null)
                {
                    foreach (string str in args)
                    {
                        send += "#" + str;
                    }
                }
                Console.WriteLine("Sent: " + send);
                writer.Write(send);
      
                // Get result from server
                string result = reader.ReadString();
                return result;
    
            }

        }

        public void close()
        {
            client.Close();
        }


    }
}
