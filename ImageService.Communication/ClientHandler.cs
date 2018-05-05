using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication.Interfaces;
using System.Net.Sockets;
using System.IO;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication.Event;
namespace ImageService.Communication
{
    public class ClientHandler:IClientHandler
    {
  //      private Dictionary<int, ICommand> commands;
        public event JsonableEvent JSEvent;
        public ClientHandler()
        {
            //commands.Add(CommandEnum.GetConfigCommand, )
        }
        //We will get a commannd for the client and by the command know what to do
        //The format is commandid#string for example let suppose 1 is for sending config it will be 1x where x is the json string of the object.
        public void HandleClient(TcpClient client)
        {
            //new Task(() =>
            //{
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                string commandLine = reader.ReadString();
                Console.WriteLine("Got input: {0}", commandLine);
                string result = "Wrong json choose"; ;
                if (commandLine.Length == 1)
                {
                    int type = int.Parse(commandLine[0] + "");
                    string json = commandLine.Substring(1);
                    Console.WriteLine("type is " + type);
                    Console.WriteLine("command is " + json);

                    Jsonable j = JSEvent?.Invoke(this, new JsonSendEventArgs(type));
                    if (j != null)
                    {
                        result = j.ToJSON();
                    }

                }
       
                //string result = ExecuteCommand(commandLine, client);
                
                writer.Write(result);
            }

            client.Close();
                //    using (NetworkStream stream = client.GetStream())
                //    using (BinaryReader reader = new BinaryReader(stream))
                //    using (BinaryWriter writer = new BinaryWriter(stream))
                //    {
                //        Console.WriteLine("Waiting for a number");
                //        int num = reader.ReadInt32();
                //        Console.WriteLine("Number accepted");
                //        num *= 2;
                //        writer.Write(num);
                //    }
                //    client.Close();
            //}).Start();
        }

        public void sendJsonable(NetworkStream stream, BinaryWriter writer, TcpClient client, Jsonable j)
        {
            string result = j.ToJSON();
                writer.Write(result);
            }
        private string ExecuteCommand(string commandLine, TcpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
