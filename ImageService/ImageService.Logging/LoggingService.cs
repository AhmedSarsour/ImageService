
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs m = new MessageRecievedEventArgs();
            //Adding the properties.
            m.Message = message;
            m.Status = type;
            //Invoking with our parameters
            MessageRecieved?.Invoke(this, m);
        }
    }
}
