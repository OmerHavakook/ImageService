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

        private void createHandler(string directory)
        {
            IDirectoryHandler handler = new DirectoryHandler(directory,m_controller, m_logging);
            // notify command
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += closeServer;


        }

        public void closeServer(object sender, DirectoryCloseEventArgs args)
        {
            m_logging.Log(args.Message, MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= closeServer;
        }

        public void createCommand(int CommandID, string[] Args, string RequestDirPath)
        {
            CommandRecievedEventArgs closeCommandArgs = new CommandRecievedEventArgs(
                CommandID, Args, RequestDirPath);
            this.CommandRecieved?.Invoke(this, closeCommandArgs);

        }

        ~ImageServer()
        {
             string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                foreach (string dir in directories)
                {
                    createCommand((int)CommandEnum.CloseCommand,null, dir);
                }
        }

        #region Properties
        #endregion


    }
#endregion
}