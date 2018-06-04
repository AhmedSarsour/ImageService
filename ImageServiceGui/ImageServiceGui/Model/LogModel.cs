using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using System.Collections;
using System.Collections.ObjectModel;
using ImageService.Infrastructure.Enums;
using ImageService.Communication.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace ImageServiceGui.Model
{
    /// <summary>
    /// LogModel class.
    /// </summary>
    class LogModel : ILogModel,INotifyPropertyChanged
    {
        private LogCollection logList;
        public ObservableCollection<Log> Logs { get; set; }
        //The class that in charge of the communication between the gui and the service
        private ModelCommunication communicate;
        private bool gotLogs = false;
        public bool IsConnected()
        {
            return communicate.IsConnected();
        }
        //If we succeed to connect to the server
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// adding a new log to the logs' list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void AddLog(object sender, string message)
        {

            Log log = new Log(1, "");

            log.FromJson(message);
            
            this.logList.AddLog(log);
            //adding the log in a way that invokes the Logs property.
            this.Logs = new ObservableCollection<Log>(logList.Logs);
            OnPropertyChanged("ListOfLogs");
        }
        /// <summary>
        /// getting the list of logs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void GetLogs(object sender, string message)
        {
            logList.FromJson(message);
            this.Logs = new ObservableCollection<Log>(logList.Logs);
            gotLogs = true;
        }
        /// <summary>
        /// the constructor.
        /// </summary>
        public LogModel()
        {
            communicate = ModelCommunication.GetInstance();

            logList = new LogCollection();

            if (!communicate.IsConnected())
            {
                return;
            }
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