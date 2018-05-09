using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    interface ISettingsModel
    {
        ObservableCollection<String> ListHandlers { get; set; }
    }
}
