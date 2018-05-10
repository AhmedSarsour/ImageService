using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using ImageService.Infrastructure.Classes;

namespace ImageService.Communication
{
    public class TcpClientChannel
    {
        private static TcpClient client;
        private NetworkStream stream = null;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static TcpClientChannel myInstance = null;
        public static bool connected;

        private static Mutex writeLock = new Mutex();
        private static Mutex readLock = new Mutex();


        public static TcpClientChannel GetInstance()
        {
            if (myInstance == null)
            {
                if (connected == false)
                {
                    myInstance = new TcpClientChannel();
                }
            }
            return myInstance;
        }
        private TcpClientChannel()
        {
            connected = false;
        }

        public static void Connect(int port)
        {
            if (!connected)
            {
                try
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                    client = new TcpClient();
                    client.Connect(ep);

                    Console.WriteLine("You are connected");
                    connected = true;
                } 
                //Problems on connecting
                catch(Exception)
                {
                    connected = false;
                }
            }
        }

        public static bool IsConnected()
        {
            return connected;
        }

        public void sendCommand(int id, string []args)
        {
            Task t = new Task(() =>
            {

                stream = client.GetStream();
            writer = new BinaryWriter(stream);
   
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
                writeLock.WaitOne();
                writer.Write(send);
                writeLock.ReleaseMutex();

                //// Get result from server
                //readLock.WaitOne();
                //string result = reader.ReadString();
                //readLock.ReleaseMutex();
                //return result;

            });
            t.Start();
//            return t.Result;

        }

        public MessageToClient recieveMessage()
        {

            Task<MessageToClient> t = new Task<MessageToClient>(() =>
            {
      
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                readLock.WaitOne();
                string result = reader.ReadString();

                MessageToClient message = new MessageToClient();
                message.FromJson(result);

                readLock.ReleaseMutex();
                // Get result from server
                return message;

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
