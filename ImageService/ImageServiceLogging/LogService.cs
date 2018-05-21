using ImageServiceInfrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceLogging
{
    public class LogService
    {
        private List<MessageRecievedEventArgs> logMsgs;
        private static LogService _instance;


        public static LogService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogService();
                }

                return _instance;
            }
        }
        public LogService()
        {
            logMsgs = new List<MessageRecievedEventArgs>();

        }

        public void addLogToList(MessageRecievedEventArgs e)
        {
            logMsgs.Insert(0, e);
        }

        public List<MessageRecievedEventArgs> LogMsgs
        {
            get { return this.logMsgs; }
        }
    }
}
