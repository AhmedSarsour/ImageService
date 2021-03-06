﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication.Event;
using ImageService.Communication.Interfaces;
using System.Threading;
using ImageService.Infrastructure.Classes;

namespace ImageService.Communication
{
    public class ClientHandler:IClientHandler
    {
        public event Excecute HandlerExcecute;
        // the mutexes for the read and write.

            /// <summary>
        /// ClientHandler's contructor, simply does nothing, only construct.
        /// </summary>
        public ClientHandler()
        {
            //commands.Add(CommandEnum.GetConfigCommand, )
        }
        /// <summary>
        ///We will get a commannd for the client and by the command know what to do
        ///The format is commandid#string for example let suppose 1 is for sending config it will be 1x where x is the json string of the object.
        /// </summary>
        /// <param name="client"> The client</param>
        /// <param name="locker">The object we want to lock </param>
        public void HandleClient(TcpClient client, object locker)
        {
            
            //creating a task that will handle the client's commands.
            Task t = new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (true)
                {
                    //Getting the command.
                    //readMutex.WaitOne();
                    string commandLine = reader.ReadString();
                   // readMutex.ReleaseMutex();
                    Console.WriteLine("Got input: {0}", commandLine);
                    //Getting id of command
                    int id = int.Parse(commandLine[0] + "");
                    string result = ExecuteCommand(commandLine, client);
                    Console.WriteLine("Write the result");
                    Console.WriteLine("Im here for  " + commandLine);
                    //Sending to the client the result - false because we handle specific client
                    MessageToClient message = new MessageToClient(id, result,false);
                    //Locking this critical place
                    lock (locker)
                    {
                        writer.Write(message.ToJSON());
                    }
                }
                //client.Close();
            });
            t.Start();
        }
        /// <summary>
        /// gets the command and client and executes the command the client gave.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <param name="client"></param>
        /// <returns></returns>
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
