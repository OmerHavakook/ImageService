using ImageService.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal.Event
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
