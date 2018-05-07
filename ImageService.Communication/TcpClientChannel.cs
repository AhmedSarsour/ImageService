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
        private static TcpClientChannel myInstance = null;

        public static TcpClientChannel GetInstance(int port)
        {
            if (myInstance == null)
            {
                myInstance = new TcpClientChannel(port);
            }
            return myInstance;
        }
        private TcpClientChannel(int port)
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
            Task<string> t = new Task<string>(() =>
            {

                stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = writer = new BinaryWriter(stream);
   
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

            });
            t.Start();
            t.Wait();
            return t.Result;

        }

        public string recieveMessage()
        {

            Task<string> t = new Task<string>(() =>
            {

                stream = client.GetStream();
                reader = new BinaryReader(stream);
                string result = reader.ReadString();
                // Get result from server
                return result;

            });
            t.Start();
            return t.Result;
        }

        public void close()
        {
            client.Close();
        }


    }
}
