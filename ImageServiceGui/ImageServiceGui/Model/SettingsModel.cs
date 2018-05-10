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
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public Configure Config { get; set; }
        public bool IsConnected()
        {
            return TcpClientChannel.connected;
        }

        private ObservableCollection<String> listHandlers;

        private void GetConfig(object sender, string message)
        {
            Config.FromJson(message);
            listHandlers = new ObservableCollection<String>(Config.Handlers);
            addedConfig = true;

        }

        private void GetHandlerClosed(object sender, string message)
        {
            return; 
        }
        public SettingsModel()
        {
            communicate = ModelCommunication.GetInstance();

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
        public void RemoveHandler(String selected)
        {
            try
            {

                communicate.SendCommend((int)CommandEnum.CloseCommand, new string[] { selected, "true" });
                MessageBox.Show("The handler for " + selected + " just closed");

               this.listHandlers.Remove(selected);
         
            }
            catch { }
        }

    }
}
