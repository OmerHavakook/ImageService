using System;
using ImageServiceInfrastructure.Enums;

namespace ImageServiceInfrastructure.Event
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            Status = status;
            Message = message;
        }

        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
