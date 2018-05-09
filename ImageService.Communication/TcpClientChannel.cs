using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ImageService.Communication
{
    public class TcpClientChannel
    {
        private static TcpClient client;
        private NetworkStream stream = null;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static TcpClientChannel myInstance = null;
        public static bool connected = false;

        private static Mutex writeLock = new Mutex();
        private static Mutex readLock = new Mutex();


        public static TcpClientChannel GetInstance()
        {
            if (myInstance == null)
            {
                myInstance = new TcpClientChannel();
            }
            return myInstance;
        }
        private TcpClientChannel()
        {
    
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

        public string sendCommand(int id, string []args)
        {
            Task<string> t = new Task<string>(() =>
            {

                stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = writer = new BinaryWriter(stream);
   
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

                // Get result from server
                readLock.WaitOne();
                string result = reader.ReadString();
                readLock.ReleaseMutex();
                return result;

            });
            t.Start();
            t.Wait();
            return t.Result;

        }

        public string recieveMessage()
        {

            Task<string> t = new Task<string>(() =>
            {

                stream = client.GetStream();
                reader = new BinaryReader(stream);
                readLock.WaitOne();
                string result = reader.ReadString();
                readLock.ReleaseMutex();
                // Get result from server
                return result;

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
