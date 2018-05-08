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
        bool Connected { get; set; }
        ObservableCollection<Log> Logs { get; set; }


    }
}
