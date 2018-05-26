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
    /// <summary>
    /// LogViewModel, connects between the Log's view and the log's model.
    /// </summary>
    class LogViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// defining the events and the properties for the View.
        /// </summary>
        private Model.LogModel model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddCommand { get; private set; }
        /// <summary>
        /// property for displaying the background.
        /// </summary>
        public string BackG { get; set; }
        /// <summary>
        /// hiding and showing things according to the connection between the client and the server.
        /// </summary>
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
        /// <summary>
        /// the constructor.
        /// </summary>
        public LogViewModel()
        {
            //creating the model.
            this.model = new Model.LogModel();
            //changing the background according to the connection.
            if (this.model.IsConnected())
            {
                BackG = "White";
            }
            else
            {
                BackG = "Gray";
                return;
            }
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

 
        /// <summary>
        /// invoking the event in case a property had changed, notifying the view.
        /// </summary>
        /// <param name="name"></param>
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
        /// <summary>
        /// the list of the logs to view.
        /// </summary>
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