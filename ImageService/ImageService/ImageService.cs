using ImageService.Controller;
using ImageService.Modal;
using ImageService.Server;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging;
using ImageServiceLogging.Logging;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {

        //DELETE AFTER DEBUGGING
        private Object thisLock = new Object();

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            System.Threading.Thread.Sleep(100000);

            //this.OnStop();
        } // UNTIL HERE


        ILoggingService logger;
        //private System.Diagnostics.EventLog eventLog;
        private int eventId = 1;
        private IImageServiceModal modal;
        private IImageController controller;
        private ImageServer server;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="args"></param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            string outputFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            this.modal = new ImageServiceModal(outputFolder, thumbnailSize);
            this.controller = new ImageController(this.modal);
            string eventSourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog.Source = eventSourceName;
            eventLog.Log = logName;
            
        }

        /// <summary>
        /// This methos is being called when the service starts
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            logger = new LoggingService();

            logger.MessageRecieved += Logger_MessageRecieved;
            this.logger.Log("On Start...", MessageTypeEnum.INFO);


            this.server = new ImageServer(controller, logger);
            logger.MessageRecieved += server.SendLog;





        }

        /// <summary>
        /// This method is being written to the MessageRecieved event and
        /// write to the logger
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Logger_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            lock (thisLock)
            {
                EventLogEntryType msg = EventLogEntryType.Information; // default
                                                                       // for error or warning msg
                switch (e.Status)
                {
                    case MessageTypeEnum.FAIL:
                        msg = EventLogEntryType.Error;
                        break;
                    case MessageTypeEnum.WARNING:
                        msg = EventLogEntryType.Warning;
                        break;

                }
                // write entry with the msg
                eventLog.WriteEntry(e.Message, msg, eventId++);
                // change to EVENT
                LogService.Instance.addLogToList(e);

            }
        }

        /// <summary>
        /// This method is being called when the service shuts down.
        /// </summary>
        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            this.logger.Log("On Stop..", MessageTypeEnum.INFO);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }
    }
}
