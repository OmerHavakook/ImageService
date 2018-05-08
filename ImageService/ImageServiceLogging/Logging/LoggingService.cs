using System;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;

namespace ImageServiceLogging.Logging
{
    public class LoggingService : ILoggingService
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
