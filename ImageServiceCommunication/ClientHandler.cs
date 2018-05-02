using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;
using System.Net.Sockets;
using System.IO;

namespace ImageServiceCommunication
{
    public class ClientHandler:IClientHandler
    {
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string commandLine = reader.ReadLine();
                Console.WriteLine("Got command: {0}", commandLine);
                //string result = ExecuteCommand(commandLine, client);
                string result = "hi";
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
            }).Start();
        }

        private string ExecuteCommand(string commandLine, TcpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
