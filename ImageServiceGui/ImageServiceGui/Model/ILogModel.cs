using ImageService.Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    interface ILogModel
    {
        ObservableCollection<Log> Logs { get; set; }


    }
}
