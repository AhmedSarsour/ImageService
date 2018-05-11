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
    class LogModel : ILogModel,INotifyPropertyChanged
    {
        private LogCollection logList;
        public ObservableCollection<Log> Logs { get; set; }
        //The class that in charge of the communication between the gui and the service
        private ModelCommunication communicate;

        private bool gotLogs = false;


        //If we succeed to connect to the server

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

   
        private void AddLog(object sender, string message)
        {

            Log log = new Log(1, "");

            log.FromJson(message);

            this.logList.AddLog(log);

            this.Logs = new ObservableCollection<Log>(logList.Logs);

            OnPropertyChanged("ListOfLogs");

        }

        private void GetLogs(object sender, string message)
        {
            logList.FromJson(message);
            this.Logs = new ObservableCollection<Log>(logList.Logs);
            gotLogs = true;
        }
        public LogModel()
        {
            communicate = ModelCommunication.GetInstance();

            logList = new LogCollection();

            try
            {
                //Request from the service the logs
                communicate.SendCommend((int)CommandEnum.LogCommand, null);
            }
            catch(Exception)
            {
                return;
            }

            //Updating the communication events
            communicate.GetLogs += GetLogs;

            communicate.AddLog += AddLog;
            //Waiting until adding the logs file until we continue.
            while (!gotLogs) ;




        }

    }
}
