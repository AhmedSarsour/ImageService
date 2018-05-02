using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Classes;
using System.Collections;

namespace ImageServiceGui.Model
{
    class LogModel: ILogModel
    {
        public LogModel()
        {
            ArrayList logs = new ArrayList();
            Log log1 = new Log("s", "ok");
            Log log2 = new Log("aa", "not ok");
            logs.Add(log1);
            logs.Add(log2);
        }
 




    }
}
