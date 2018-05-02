using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication.Classes
{
    public class Log
    {
        public string status;

        //The source name in the event log
        public string Info { get; private set; }
        //The log name
        public string Status { get; private set; }

        public Log(string info, string status)
        {
            this.Info = info;
            this.Status = status;
        }
    }
}
