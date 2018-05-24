using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using System.Collections.ObjectModel;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using System.Windows;

namespace ImageServiceGui.Model
{
    class SettingsModel : ISettingsModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //The class that in charge of the communication between the gui and the service
        private IModelCommunication communicate;

        //True if we finished adding the config file
        private bool addedConfig = false;
        private string removedFolder = "";
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool IsConnected()
        {
            return communicate.IsConnected();
        }

        public Configure Config { get; set; }


        private ObservableCollection<String> listHandlers;

        private void GetConfig(object sender, string message)
        {
            Config.FromJson(message);
            listHandlers = new ObservableCollection<String>(Config.Handlers);
            addedConfig = true;

        }

        private void GetHandlerClosed(object sender, string message)
        {
            bool allClients = false;
            if (sender is bool)
            {
                allClients = (bool)sender;
            }
            //We know that the message from close command will be the handler of the folder x just closed.
            //We want to get x.
            string first = "The handler of the folder ";
            int pFrom = message.IndexOf(first) + first.Length;

            int pTo = message.LastIndexOf(" just closed");
            string folderToClose = "";
            //Found the folders
            if (pFrom != -1 && pTo != -1)
            {
                folderToClose = message.Substring(pFrom, pTo - pFrom);
                //We did not remove it already

                if (removedFolder != folderToClose)
                {
                    RemoveHandler(allClients, folderToClose);
                }
             

            }

        }
        public SettingsModel()
        {
            communicate = ModelCommunication.GetInstance();

            if (!communicate.IsConnected())
            {
                return;
            }
            Config = Configure.GetInstance();

            //Request from the service the configurations

            try
            {
                communicate.SendCommend((int)CommandEnum.GetConfigCommand, null);
                communicate.GetConfig += GetConfig;
                communicate.RemoveHandler += GetHandlerClosed;
                //Waiting until adding the config file until we continue;
                while (!addedConfig) ;
            }
            catch (Exception)
            {

                return;
            }



        }
        public ObservableCollection<String> ListHandlers
        {
            get { return this.listHandlers; }
            set { this.listHandlers = value; }
        }
        private string m_SelectedItem;
        public string SelectedItem
        {
            get { return m_SelectedItem; }
            set
            {
                m_SelectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }
        public void RemoveHandler(object sender, String selected)
        {
            try
            {
                bool allClients = true;
                if (sender is bool)
                {
                    allClients = (bool)sender;
                }
                if (!allClients)
                {
                    communicate.SendCommend((int)CommandEnum.CloseCommand, new string[] { selected, "true" });
                }
                else
                {
                  //  this.SelectedItem = selected;

                    this.SelectedItem = null;
                }
                removedFolder = selected;

                //Invoking all the threads
                Application.Current.Dispatcher.Invoke(() => this.listHandlers.Remove(selected));


            }
            catch { }
        }

    }
}
