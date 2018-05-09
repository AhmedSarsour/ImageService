using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using System.Collections;
using System.Collections.ObjectModel;
using ImageService.Infrastructure.Enums;
using ImageService.Communication;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace ImageServiceGui.Model
{
    class LogModel : ILogModel
    {
        private LogCollection logList;
        public ObservableCollection<Log> Logs { get; set; }



        //If we succeed to connect to the server
        public bool Connected { get; set; }

        public LogModel()
        {


            logList = new LogCollection();

            TcpClientChannel client = TcpClientChannel.GetInstance();
            try
            {

                TcpClientChannel.Connect(8000);
                string logsJson = client.sendCommand((int)CommandEnum.LogCommand, null);
                logList.FromJson(logsJson);
                Connected = true;


            }
            catch (Exception)
            {
                logList.AddLog(new Log((int)MessageTypeEnum.FAIL, "Did not connected.."));
                Connected = false;
            }

            this.Logs = new ObservableCollection<Log>(logList.Logs);
            if (Connected)
            {
                try
                {
                    Task t = new Task(() =>
                    {
                        while (true)
                        {

                            //We need a thread to read from socket while being on the gui.
                            string newLog = "";
                            //Task t2 = new Task(() =>
                            //{
                            newLog = client.recieveMessage();
                            //});
                            //t2.Start();
                            //t2.Wait();
                         //   MessageBox.Show("Finaly dudde");


                            Log log = new Log(1, "");

                            log.FromJson(newLog);
                            //  MessageBox.Show("Log is " + log.Message);

                            //logObservable.Add(log);
                            //When receiving new log we will 
                            this.Logs.Add(log);

                        }
                    });

                    t.Start();
                    //   t.Wait();


                    //  t.Wait();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Bro it is not connected!");
            }

        }

    }
}
