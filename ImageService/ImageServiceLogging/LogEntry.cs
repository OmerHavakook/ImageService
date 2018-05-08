using ImageServiceInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceLogging
{
    public class LogEntry
    {
        public string msg { get; set; }
        public MessageTypeEnum type { get; set; }

        public LogEntry(MessageTypeEnum type, string message)
        {
            this.type = type;
            this.msg = message;
        }
    }
}
