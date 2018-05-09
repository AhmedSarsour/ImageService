using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageServiceGui.ViewModel
{
    class LogViewModel : INotifyPropertyChanged
    {
        private Model.ILogModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddCommand { get; private set; }


        public LogViewModel()
        {
            this.model = new Model.LogModel();
            this.model.Logs.CollectionChanged += (sender, args) => NotifyPropertyChanged("ListOfLogs");
            


        }
        public void NotifyPropertyChanged(string propName)
        {
            MessageBox.Show("Count" + ListOfLogs.Count);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public ObservableCollection<Log> ListOfLogs
        {

            get
            {
                return model.Logs;
            }
        }


    }
}