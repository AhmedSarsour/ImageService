using ImageService.Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    //an interface for the settings model.
    interface ISettingsModel
    {
        //defining the properties and events we need.
         ObservableCollection<String> ListHandlers { get; set; }
         event PropertyChangedEventHandler PropertyChanged;

         bool IsConnected();

         Configure Config { get; set; }

         string SelectedItem { get; set; }

        void RemoveHandler(object sender, string handler);

    }
}
