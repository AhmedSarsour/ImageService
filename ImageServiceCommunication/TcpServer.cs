using ImageServiceCommunication.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class TcpServer : IServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        public TcpServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
        }
        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for connections...");
            Task task = new Task(() =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    try
                    {
                        Console.WriteLine("Got new connection");
                     //   ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();


            //Task task = new Task(() =>
            //{
            //    TcpClient client = listener.AcceptTcpClient();
            //    Console.WriteLine("Got new connection");

            //});
            //task.Start();

            //   // ch.HandleClient(client);

            //    //using (NetworkStream stream = client.GetStream())
            //    //using (BinaryReader reader = new BinaryReader(stream))
            //    //using (BinaryWriter writer = new BinaryWriter(stream))
            //    //{
            //    //    Console.WriteLine("Waiting for a number");
            //    //    int num = reader.ReadInt32();
            //    //    Console.WriteLine("Number accepted");
            //    //    num *= 2;
            //    //    writer.Write(num);
            //    //}
            //    //client.Close();
        }
        public void Stop()
        {
            listener.Stop();
        }

    }
}
