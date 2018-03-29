using ImageService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
     class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved


        public ImageServer(IImageController m_controller, ILoggingService m_logging)
        {
            
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string dir in directories)
            {
                createHandler(dir);
            }

        }

        public void invokeCommand(CommandRecievedEventArgs commandArgs)
        {
            CommandRecieved?.Invoke(this, commandArgs);
        }

        private void createHandler(string directory)
        {
            IDirectoryHandler handler = new DirectoryHandler(directory,m_controller, m_logging);
            // notify command
            CommandRecieved += handler.OnCommandRecieved;


        }
        #endregion

        #region Properties
        #endregion

        
    }
}