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
    /// <summary>
    /// TcpClientChannel class.
    /// </summary>
    public class TcpClientChannel
    {
        private static TcpClient client;
        private NetworkStream stream = null;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static TcpClientChannel myInstance = null;
        public static bool connected;

        /// <summary>
        /// a singleton for the tcpClientChannel.
        /// </summary>
        /// <returns></returns>
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
        //private constructor that is used in the singleton.
        private TcpClientChannel()
        {
            connected = false;
        }
        /// <summary>
        /// connecting to the client according to the given port.
        /// </summary>
        /// <param name="port"></param>
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
                    throw new Exception("Problem connecting");
                }
            }
        }
        /// <summary>
        /// a function that knows if the connection was successful or not.
        /// </summary>
        /// <returns></returns>
        public static bool IsConnected()
        {
            return connected;
        }

        /// <summary>
        /// a function that sends the arguments to the server.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public void sendCommand(int id, string []args)
        {
            if (!connected)
            {
                return;
            }
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
                writer.Write(send);
            });
            t.Start();
        }
        /// <summary>
        /// a function that handles the receiving of the incomming messages from the server.
        /// </summary>
        /// <returns></returns>
        public MessageToClient recieveMessage()
        {
            Task<MessageToClient> t = new Task<MessageToClient>(() =>
            {
      
                stream = client.GetStream();
                reader = new BinaryReader(stream);
                string result;
                try
                {
                    result = reader.ReadString();
                }
                catch (Exception)
                {
                    connected = false;
                    return null;
                }
                MessageToClient message = new MessageToClient();
                message.FromJson(result);
                // Get result from server
                return message;

            });
            t.Start();

            if (t.Result == null)
            {
                throw new Exception("Problem connecting");
            }
            return t.Result;
        }
        /// <summary>
        /// close function that closes the client.
        /// </summary>
        public void close()
        {
            client.Close();
        }
    }
}
