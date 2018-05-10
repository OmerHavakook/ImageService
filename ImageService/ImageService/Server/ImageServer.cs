using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging.Logging;
using ImageServiceCommunication;
using System;
using System.Configuration;
using System.IO;

namespace ImageService.Server
{
    class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        private TcpServerChannel serverChannel;



        public ImageServer(IImageController m_controller, ILoggingService m_logging)
        {

            this.m_controller = m_controller;
            this.m_logging = m_logging;
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string dir in directories)
            {
                if (Directory.Exists(dir))
                {
                    createHandler(dir);
                }
                else
                {
                    m_logging.Log("Not such file or directory: " + dir, MessageTypeEnum.FAIL);
                }
            }
            this.serverChannel = new TcpServerChannel(8000, "127.0.0.1");
        }

        public void start()
        {
            serverChannel.Start();
            serverChannel.MessageRecived += GetMessageFromUser;
        }

        private void GetMessageFromUser(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg.CommandId == (int)CommandEnum.CloseCommand)
            {
                CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand,
                    null, msg.Args[0]));

                serverChannel.SendToAll(msg.ToJson());
            } else
            {
                bool result;
                string answer =m_controller.ExecuteCommand(msg.CommandId, null,out result);
                serverChannel.SendToAll(answer);
            }
        }

        /// <summary>
        /// create new directory handler.
        /// </summary>
        /// <param name="directory"></param>
        private void createHandler(string directory)
        {
            IDirectoryHandler handler = new DirectoryHandler(directory, m_controller, m_logging);
            // notify command
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += closeServer;
        }

        /// <summary>
        /// The function inform the server that a handler is done handle the dir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void closeServer(object sender, DirectoryCloseEventArgs args)
        {
            m_logging.Log(args.Message, MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
            handler.DirectoryClose -= closeServer;
        }

        /// <summary>
        /// create the command and invoke the event command recieved.
        /// </summary>
        /// <param name="CommandID"></param>
        /// <param name="Args"></param>
        /// <param name="RequestDirPath"></param>
        public void createCommand(int CommandID, string[] Args, string RequestDirPath)
        {
            CommandRecievedEventArgs closeCommandArgs = new CommandRecievedEventArgs(
                CommandID, Args, RequestDirPath);
            this.CommandRecieved?.Invoke(this, closeCommandArgs);

        }

        /// <summary>
        /// distructor when the server is closed.
        /// </summary>
        ~ImageServer()
        {
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string dir in directories)
            {
                if (Directory.Exists(dir))
                {
                    createCommand((int)CommandEnum.CloseCommand, null, dir);
                }

            }
        }

        #region Properties
        #endregion
    }
    #endregion
}