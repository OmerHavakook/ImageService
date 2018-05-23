using System;
using ImageServiceInfrastructure.Enums;

namespace ImageServiceInfrastructure.Event
{
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="status"></param> type MessageTypeEnum
        /// <param name="message"></param> type string
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            Status = status;
            Message = message;
        }

        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
