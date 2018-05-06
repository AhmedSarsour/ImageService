using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication.Event;
using ImageService.Communication.Interfaces;
namespace ImageService.Communication
{
    public class ClientHandler:IClientHandler
    {
        public event Excecute HandlerExcecute;
        public ClientHandler()
        {
            //commands.Add(CommandEnum.GetConfigCommand, )
        }
        //We will get a commannd for the client and by the command know what to do
        //The format is commandid#string for example let suppose 1 is for sending config it will be 1x where x is the json string of the object.
        public void HandleClient(TcpClient client)
        {

            Task t = new Task(() =>
            {

                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (true)
                {
                    //Getting the command.
                    string commandLine = reader.ReadString();
                    Console.WriteLine("Got input: {0}", commandLine);
                    string result = ExecuteCommand(commandLine, client);
                    Console.WriteLine("Write the result");
                    writer.Write(result);

                }
                //client.Close();
            });
            t.Start();


        }

        public void sendJsonable(NetworkStream stream, BinaryWriter writer, TcpClient client, Jsonable j)
        {
            string result = j.ToJSON();
                writer.Write(result);
            }
        private string ExecuteCommand(string commandLine, TcpClient client)
        {
            string[] args = null;
            string result = "Wrong json choose";
            bool boolRes;

            //Getting id and arguments from the command line
            int id = int.Parse(commandLine[0] + "");
            if (commandLine.Length > 2)
            {
                //The args are after the first # it means after index 2
                args = commandLine.Substring(2).Split('#');
            }
            //Sending the command to the server
            result = HandlerExcecute?.Invoke(id, args, out boolRes);

            return result;


        }
    }
}
