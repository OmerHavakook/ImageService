using ImageService.Enums;
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// write messages to the eventlogger of the program.
        /// </summary>
        /// <param name="message">The wanted message.</param>
        /// <param name="type">The type of the message.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this,new MessageRecievedEventArgs(type,message));
        }
    }
}
