using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using System.Collections.ObjectModel;

namespace ImageServiceGui.Model
{
    class SettingsModel : ISettingsModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public string outPutFolder { get; set; }
        public string sourceName { get; set; }
        public string logName { get; set; }
        public string ThumbnailSize { get; set; }
        private ObservableCollection<String> listHandlers;
        public SettingsModel()
        {
            Configure config = Configure.GetInstance();
            listHandlers = new ObservableCollection<String>();
            listHandlers.Add("handler1");
            listHandlers.Add("handler2");
            listHandlers.Add("handler3");
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
                this.listHandlers.Remove(selected);
            }
            catch { }
        }

    }
}
