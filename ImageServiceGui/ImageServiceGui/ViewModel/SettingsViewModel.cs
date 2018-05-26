using ImageService.Infrastructure.Classes;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ImageServiceGui.ViewModel
{
    /// <summary>
    /// SettingsViewModel: it connects betwee nthe setttings View and the settings Model.
    /// </summary>
    class SettingsViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private Model.ISettingsModel sModel;
        #endregion
        //The properties we want to bind in the settings View.
        public string outPutFolder { get; set; }
        public string sourceName { get; set; }
        public string logName { get; set; }
        public string ThumbnailSize { get; set; }
        public string BackG { get; set; }
        //HideOrVis property changes the view based on if connection failed or succeeded between server and client(GUI).
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
        /// the removeCommand property for the remove button.
        /// </summary>
        public ICommand RemoveCommand { get; private set; }

        public SettingsViewModel()
        {
            //creating the settings model.
            this.sModel = new Model.SettingsModel();
            //check whether the connection was successfully made or failed.
            if (this.sModel.IsConnected()) {
                //the connection was successful, getting the information from the config.
                BackG = "White";
                Configure config = sModel.Config;
                outPutFolder = config.OutPutDir;
                sourceName = config.SourceName;
                logName = config.LogName;
                ThumbnailSize = config.ThumbnailSize.ToString();
            } else {
                //the connection failed...
                BackG = "Gray";
                return;
            }
            //applying the conditions and effects of the remove button.
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            PropertyChanged += Handler_Remove;
            this.sModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged(e.PropertyName); };
        }
        /// <summary>
        /// invokes the OnRemove func according to the CanRemove.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Handler_Remove(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();
        }
        /// <summary>
        /// property for the selectedItem in the listbox.
        /// </summary>
        public string SelectedItem
        {
            get { return this.sModel.SelectedItem; }
            set
            {
                this.sModel.SelectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }
        /// <summary>
        /// closing the mainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void close(object sender, EventArgs e)
        {
            MessageBox.Show("close");
        }
        /// <summary>
        /// OnRemove; fires when the button was clicked.
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemove(object obj)
        {
            this.sModel.RemoveHandler(false,this.SelectedItem);
            this.sModel.SelectedItem = null;
        }
        /// <summary>
        /// CanRemove; definning the right time when the button can be clicked.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CanRemove(object obj)
        {
            return this.sModel.ListHandlers.Contains(this.SelectedItem);
        }
        /// <summary>
        /// ListOfHandlers property in order to bind to the handlers observation list in a listbox.
        /// </summary>
        public ObservableCollection<String> ListOfHandlers
        {
            get { return this.sModel.ListHandlers; }
        }
    }
}
 