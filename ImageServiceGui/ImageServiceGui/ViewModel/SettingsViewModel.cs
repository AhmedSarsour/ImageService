using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImageServiceGui.ViewModel
{
    class SettingsViewModel:INotifyPropertyChanged
    {
        #region Notify Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            throw new NotImplementedException();
        }
        #endregion
        //The properties we want to bind into
        public string outPutFolder { get; set; }
        public string sourceName { get; set; }
        public string logName { get; set; }
        public string ThumbnailSize { get; set; }

    }
}
