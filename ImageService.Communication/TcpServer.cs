using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication.Event;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication.Interfaces;
using ImageService.Infrastructure.Classes;

namespace ImageService.Communication
{


    public class TcpServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        //We will register to this event a function who will excecute the command
        public delegate string Exec(int commandID, string[] args, out bool resultSuccesful);
        //Creating event
        public event Exec ExcecuteCommand;
        private List<TcpClient> clients;
        //Singelton
        private static TcpServer myInstance = null;
        public static TcpServer GetInstance(int port, IClientHandler ch)
        {
            if (myInstance == null)
            {
                myInstance = new TcpServer(port, ch);
            }
            return myInstance;
        }
        private TcpServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            //We want to raise it from client handler so we register into it.
            //The handler will call to the function execute.
            ch.HandlerExcecute += Execute;
            //Initialize the list
            this.clients = new List<TcpClient>();
        }
        //Maybe reference if not synchronized!


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
                        this.clients.Add(client);
                        Console.WriteLine("Got new connection");

                            ch.HandleClient(client);

                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();

        }
        public void Stop()
        {
            listener.Stop();
        }

        //We will call the original controller to excecute the command
        public string Execute(int commandID, string[] args, out bool resultSuccesful)
        {
            resultSuccesful = true;
            bool result;
            return ExcecuteCommand?.Invoke(commandID, args, out result);
        }
        //Sending message to all clients
        public void SendToAllClients(object sender, MessageRecievedEventArgs e)
        {

            foreach (TcpClient client in clients)
            {
                Task t = new Task(() =>
                {
                    Log log = new Log((int)e.Status, e.Message);
                    SendMessage(log.ToJSON(), client);
                });
                t.Start();
                t.Wait();
            }
        }
        public void SendMessage(string message, TcpClient client)
        {
            Task<string> t = new Task<string>(() =>
            {
                 NetworkStream stream = client.GetStream();
                BinaryReader reader =  new BinaryReader(stream);
                BinaryWriter writer =  new BinaryWriter(stream);
                writer.Write(message);

                // Get result from server
                string result = reader.ReadString();
                return result;

            });
            t.Start();
         //   return t.Result;

            //The sender is the jsonable
        }




    }
}
