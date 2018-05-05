
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// The publisher event, MessageRecieved.
        /// </summary>
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// setting the messages of the log once the message recieved.
        /// </summary>
        /// <param name="message">The message we want to write on the log</param>
        /// <param name="type">The type of message (success, fail...)</param>
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
