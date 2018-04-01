using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// A logging service which can write logs.
    /// </summary>
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Logging the service
        /// </summary>
        /// <param name="message">The message we want to write on the log</param>
        /// <param name="type">The type of message (success, fail...)</param>
        void Log(string message, MessageTypeEnum type);           
    }
}
