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
        public string Type { get; private set; }
        //The log name
        public string Message { get; private set; }

        public Log(string type, string message)
        {
            this.Type = type;
            this.Message = message;
        }
    }
}
