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
using System.Threading;
using ImageService.Infrastructure.Enums;

namespace ImageService.Communication
{

    /// <summary>
    /// TcpServer class.
    /// </summary>
    public class TcpServer
    {
        private static object locker;
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
        /// <summary>
        /// TcpServer's singleton.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static TcpServer GetInstance(int port, IClientHandler ch)
        {
            if (myInstance == null)
            {
                myInstance = new TcpServer(port, ch);
            }
            return myInstance;
        }

        public static TcpServer GetInstance()
        {
            if (myInstance == null)
            {
                return null;
            }
            return myInstance;
        }
        /// <summary>
        /// a private constructor that is used in the singleton.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ch"></param>
        private TcpServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            //We want to raise it from client handler so we register into it.
            //The handler will call to the function execute.
            ch.HandlerExcecute += Execute;
            //Initialize the list
            this.clients = new List<TcpClient>();
            locker = new object();
        }
        //note to self: Maybe reference if not synchronized!
        /// <summary>
        /// Starting the server, waiting for clients.
        /// </summary>
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
                    //waiting for clients to connect.
                    TcpClient client = listener.AcceptTcpClient();

                    try
                    {
                        //we have got a client so we add it to the client's list.
                        this.clients.Add(client);
                        Console.WriteLine("Got new connection");
                        //handling the client.
                        ch.HandleClient(client, locker);
                    }
                    catch (SocketException)
                    {
                        //Remove the client from the list of clients.
                        break;
                    }
                }
            Console.WriteLine("Server stopped");
            });
            task.Start();
        }
        /// <summary>
        /// stopping the server.
        /// </summary>
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
        public void SendToAllClients(int type, string content)
        {
            new Task(() =>
            {
                foreach (TcpClient client in clients)
                {
                    if (!client.Connected)
                    {
                        continue;
                    }
                    //It is true because this message is sent to all client
                    SendMessage(type, content, client, true);
                }
            }).Start();
        }
        /// <summary>
        /// sending messages to all the clients.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="client"></param>
        /// <param name="allClients"></param>
        public void SendMessage(int type, string content, TcpClient client, bool allClients)
        {
            NetworkStream stream = client.GetStream();
            BinaryReader reader =  new BinaryReader(stream);
            BinaryWriter writer =  new BinaryWriter(stream);
            //Convert to type of message
            MessageToClient message = new MessageToClient(type, content, allClients);
            //Locking this critical place
            lock (locker)
            {
                writer.Write(message.ToJSON());
            }
        }
    }
}
