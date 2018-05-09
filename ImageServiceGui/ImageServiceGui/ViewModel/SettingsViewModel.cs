using ImageService.Infrastructure.Classes;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGui.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        Model.SettingsModel sModel;
        #endregion
        //The properties we want to bind into
        public string outPutFolder { get; set; }
        public string sourceName { get; set; }
        public string logName { get; set; }
        public string ThumbnailSize { get; set; }
        public ICommand RemoveCommand { get; private set; }
        public SettingsViewModel()
        {
            this.sModel = new Model.SettingsModel();

            if (this.sModel.IsConnected())
            {
                Configure config = sModel.Config;
                outPutFolder = config.OutPutDir;
                sourceName = config.SourceName;
                logName = config.LogName;
                ThumbnailSize = config.ThumbnailSize.ToString();
            }
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            PropertyChanged += Handler_Remove;
            this.sModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { NotifyPropertyChanged(e.PropertyName); };
        }

        private void Handler_Remove(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();
        }

        public string SelectedItem
        {
            get { return this.sModel.SelectedItem; }
            set
            {
                this.sModel.SelectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        private void OnRemove(object obj)
        {
            this.sModel.RemoveHandler(this.SelectedItem);
            this.sModel.SelectedItem = null;
        }

        private bool CanRemove(object obj)
        {
            return this.sModel.ListHandlers.Contains(this.SelectedItem);
        }
        public ObservableCollection<String> ListOfHandlers
        {
            get { return this.sModel.ListHandlers; }
        }
    }
}
 