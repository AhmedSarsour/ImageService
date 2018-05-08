using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using System.Collections;
using System.Collections.ObjectModel;
using ImageService.Infrastructure.Enums;

namespace ImageServiceGui.Model
{
    class LogModel : ILogModel
    {
        private LogCollection logList;
        private ObservableCollection<Log> logObservable;
        public LogModel()
        {
            logList = new LogCollection();
            Log log1 = new Log((int)MessageTypeEnum.INFO, "you are a MAN");
            Log log2 = new Log((int)MessageTypeEnum.INFO, "Okay");
            Log log3 = new Log((int)MessageTypeEnum.WARNING, "not ok");
            Log log4 = new Log((int)MessageTypeEnum.FAIL, "ERROR!!!!!!");
            Log log5 = new Log((int)MessageTypeEnum.WARNING, "this one doesn't matter");
            Log log6 = new Log((int)MessageTypeEnum.INFO, "hello again!");
            logList.AddLog(log1);
            logList.AddLog(log2);
            logList.AddLog(log3);
            logList.AddLog(log4);
            logList.AddLog(log5);
            logList.AddLog(log6);

            this.logObservable = new ObservableCollection<Log>(logList.Logs);
        }

        public ObservableCollection<Log> Logs
        {
            get
            {
                return this.logObservable;
            }

            set
            {
                this.logObservable = value;
            }
        }
    }
}
