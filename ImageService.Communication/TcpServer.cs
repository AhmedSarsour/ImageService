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

namespace ImageService.Communication
{


    public class TcpServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private Dictionary<int, Jsonable> jsons;
        //We will register to this event a function who will excecute the command
        public delegate string Exec(int commandID, string[] args, out bool resultSuccesful);
        public event Exec ExcecuteCommand;
        //Creating event


        public TcpServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
            this.jsons = new Dictionary<int, Jsonable>();
            //We want to raise it from client handler so we register into it.
            //The handler will call to the function execute.
            ch.HandlerExcecute += Execute;   
        }
        //Maybe reference if not synchronized!
        public void AddJsonAble(int id, Jsonable j)
        {
            jsons.Add(id, j);
        }
        //If contains the key true else false
        public Jsonable GetJsonAble(object sender, JsonSendEventArgs args)
        {
            try
            {
                return jsons[args.Id];
              
            }
            //If the key does not exist we will return null
            catch (KeyNotFoundException e)
            {
                return null;
            }
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
                       
                            ch.HandleClient(client);
                        Console.WriteLine("Im here");

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




    }
}
