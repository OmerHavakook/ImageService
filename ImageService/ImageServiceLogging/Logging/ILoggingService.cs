using System;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;

namespace ImageServiceLogging.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}
