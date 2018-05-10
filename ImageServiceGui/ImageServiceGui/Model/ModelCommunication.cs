using ImageService.Communication;
using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageServiceGui.Model
{
    class ModelCommunication : IModelCommunication
    {
        public event EventHandler<string> AddLog;
        public event EventHandler<string> GetConfig;
        public event EventHandler<string> GetLogs;
        public event EventHandler<string> RemoveHandler;

        private TcpClientChannel client;
        private static ModelCommunication instance = null;
        public void SendCommend(int id, string [] args)
        {
            client.sendCommand(id, args);
        }

        public static ModelCommunication GetInstance()
        {
            if (instance == null)
            {
                instance = new ModelCommunication();
            }

            return instance;

        }
        private ModelCommunication()
        {
            client = TcpClientChannel.GetInstance();
            if (!TcpClientChannel.connected)
            {
                try
                {

                    TcpClientChannel.Connect(8000);
                }
                catch (Exception)
                {

                }
            }
            //If we connected we will continue
            if (TcpClientChannel.connected)
            {
                try
                {
                    Task t = new Task(() =>
                    {
                        //Receiving messages v
                        while (true)
                        {

                            MessageToClient message;
                            message = client.recieveMessage();


                            int id = message.TypeMessage;
                            string content = message.Content;
                            //MessageBox.Show("Id is " + id);
                            //MessageBox.Show("Content is " + content);
                            //Check to who transfer the message

                            if (id == (int)SendClientEnum.AddLog)
                            {
                                AddLog?.Invoke(this, content);
                            }

                            if (id == (int)SendClientEnum.RemoveHandler)
                            {
                                RemoveHandler?.Invoke(this, content);
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
                catch (Exception)
                {

                }
            }
        }
    }
}
