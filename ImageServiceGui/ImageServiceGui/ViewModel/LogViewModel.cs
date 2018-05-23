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
        private Model.LogModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddCommand { get; private set; }
        public string BackG { get; set; }
        public string HideOrVis
        {
            get
            {
                if (BackG == "White")
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
        }

        public LogViewModel()
        {
            this.model = new Model.LogModel();
            if (this.model.IsConnected())
            {
                BackG = "White";
            }
            else
            {
                BackG = "Gray";
                return;
            }
            // this.model.Logs.CollectionChanged += (sender, args) => NotifyPropertyChanged("ListOfLogs");
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

 

        public void NotifyPropertyChanged(string name)
        {
            //string str = "";

            //foreach (Log log in ListOfLogs)
            //{
            //    str += "log is " + log.Message + '\n';
            //}
            //MessageBox.Show(str);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Log> ListOfLogs
        {

            get
            {
                return model.Logs;
            }

            set
            {
                this.ListOfLogs = value;
            }
        }


    }
}