using ImageServiceInfrastructure.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceLogging
{
    /// <summary>
    /// We made this class a singleton in order to get the same list
    /// of msgs each time, moreover we needed a flexible way to save
    /// the all logs
    /// </summary>
    public class LogService
    {
        private List<MessageRecievedEventArgs> logMsgs;
        private static LogService _instance;
        
        /// <summary>
        /// static method which returns the class instance (if instance is
        /// null than it initializes it)
        /// </summary>
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

        /// <summary>
        /// c'tor
        /// </summary>
        public LogService()
        {
            logMsgs = new List<MessageRecievedEventArgs>();
        }

        /// <summary>
        /// add new log to the list of logs
        /// </summary>
        /// <param name="e"></param>
        public void addLogToList(MessageRecievedEventArgs e)
        {
            logMsgs.Insert(0, e);
        }

        /// <summary>
        /// Property for the list of logs
        /// </summary>
        public List<MessageRecievedEventArgs> LogMsgs
        {
            get { return this.logMsgs; }
        }
    }
}
