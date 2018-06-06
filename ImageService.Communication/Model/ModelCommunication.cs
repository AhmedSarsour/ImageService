using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageService.Communication.Model
{
    /// <summary>
    /// class for the communation between server and client.
    /// </summary>
    public class ModelCommunication : IModelCommunication
    {
        /// declareing events that we will use.
        public event EventHandler<string> AddLog;
        public event EventHandler<string> GetConfig;
        public event EventHandler<string> GetLogs;
        public event EventHandler<string> RemoveHandler;
        private TcpClientChannel client;
        private static ModelCommunication instance = null;
        private static bool connected = false;
        /// <summary>
        /// SendCommend, sending the command to the client.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public void SendCommend(int id, string [] args)
        {
            client.sendCommand(id, args);
        }
        /// <summary>
        /// singlton.
        /// </summary>
        /// <returns></returns>
        public static ModelCommunication GetInstance()
        {
            if (instance == null || connected == false)
            {
                instance = new ModelCommunication();
            }

            return instance;

        }
        /// <summary>
        /// boolean determines the connectivity between the client and the server.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return connected;
        }
        /// <summary>
        /// the constructor.
        /// </summary>
        private ModelCommunication()
        {
            ///getting a tcpClient instance.
            client = TcpClientChannel.GetInstance();
            //if not already connected then connect.
            if (!TcpClientChannel.connected)
            {
                try
                {
                    //connecting to the server and setting the connected boolean to true.
                    TcpClientChannel.Connect(8000);
                    connected = true;
                }
                catch (Exception)
                {
                    //connection failed.
                    connected = false;
                }
            }
            //If we connected we will continue
            if (TcpClientChannel.connected)
            {
                try
                {
                    Task t = new Task(() =>
                    {
                        //Receiving messages.
                        while (true)
                        {
                            MessageToClient message;
                            //getting all things that relate to the message.
                            try
                            {
                                message = client.recieveMessage();
                            } catch(Exception)
                            {
                                connected = false;
                                return;
                               
                            }
                            int id = message.TypeMessage;
                            string content = message.Content;
                            bool allClients = message.AllClients;
                            //Check to who transfer the message
                            if (id == (int)SendClientEnum.AddLog)
                            {
                                AddLog?.Invoke(this, content);
                            }
                            if (id == (int)SendClientEnum.RemoveHandler)
                            {
                                RemoveHandler?.Invoke(allClients, content);
                            }

                            if (id == (int)SendClientEnum.GetConfig)
                            {
                                GetConfig?.Invoke(this, content);
                            }

                            if (id == (int)SendClientEnum.GetLogs)
                            {
                                GetLogs?.Invoke(this, content);
                            }
                        }
                    });
                    t.Start();
                }
                catch (Exception){
                    connected = false;
                }
            }
        }
    }
}
