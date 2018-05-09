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
        private TcpClientChannel client;
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
        public SettingsModel()
        {
            client = TcpClientChannel.GetInstance();
            Config = Configure.GetInstance();

            try
            {

                TcpClientChannel.Connect(8000);
                string configJson = "";
                    configJson = client.sendCommand((int)CommandEnum.GetConfigCommand, null);
                Config.FromJson(configJson);
            }
            catch (Exception)
            {

                return;
            }


            listHandlers = new ObservableCollection<String>(Config.Handlers);

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
                client.sendCommand((int)CommandEnum.CloseCommand,new string[] { selected, "true" });
                MessageBox.Show("The handler for " + selected + " just closed");

                this.listHandlers.Remove(selected);
            }
            catch { }
        }

    }
}
