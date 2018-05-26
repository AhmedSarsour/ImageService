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
    //the LogModel interfance.
    interface ILogModel
    {
        //defining the evetns and properties we need.
        ObservableCollection<Log> Logs { get; set; }
         event PropertyChangedEventHandler PropertyChanged;
    }
}
