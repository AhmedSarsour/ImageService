using ImageService.Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.ViewModel
{
    class LogViewModel
    {
        Model.ILogModel model;
        public LogViewModel()
        {
            this.model = new Model.LogModel();
        }
        public ObservableCollection<Log> ListOfLogs
        {
            get { return model.Logs; }
            set
            {
                model.Logs = value;
            }
        }

        /*
        public string getInfo(int i)
        {
            return model.Logs[i].Info;
        }
        public string getMessage(int i)
        {
            return model.Logs[i].Status;
        }
        */
    }
}