using ImageService.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace ImageService.Communication
{
    public class AndroidHandler : IClientHandler
    {
        public event Excecute HandlerExcecute;
        public AndroidHandler()
        {

        }
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
                    byte[] readBytes = reader.ReadBytes(1);

                    if (readBytes[0] == 1)
                    {
                        readBytes = reader.ReadBytes(4);
                        //First send byte who specify the size

                        Console.WriteLine("Got from user:");
                        for (int i = 0; i < readBytes.Length; i++)
                        {
                            Console.Write(readBytes[i] + " ");
                        }

                    } 
                    //Finish connection
                    if (readBytes[0] == 2)
                    {
                        Console.WriteLine("Finished");
                        break;
                    }
                    //Locking this critical place
                    //lock (locker)
                    //{
                    //    writer.Write(message.ToJSON());
                    //}
                }
                //client.Close();
            });
            t.Start();




        }
    }
}
