using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging.Logging;
using ImageServiceCommunication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ImageServiceLogging;

namespace ImageService.Server
{
    class ImageServer
    {
        #region Members
        private readonly IImageController _mController;
        private readonly ILoggingService _mLogging;
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        private TcpServerChannel serverChannel;
        private Object thisLock = new Object();

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="m_controller"></param> to execute commands from its dictionary
        /// <param name="m_logging"></param> to send logs
        public ImageServer(IImageController m_controller, ILoggingService m_logging)
        {
            this._mController = m_controller;
            this._mLogging = m_logging;
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string dir in directories)
            {
                if (Directory.Exists(dir))
                {
                    createHandler(dir);
                }
                else
                {   // if dir is not exists
                    bool result;
                    string[] args = { dir };
                    m_controller.ExecuteCommand((int)CommandEnum.CloseCommand, args, out result);
                    m_logging.Log("Not such file or directory: " + dir, MessageTypeEnum.WARNING);
                }
            }

            // get comunication details to connect the server
            string ip = ConfigurationManager.AppSettings.Get("Ip");
            int port = Int32.Parse(ConfigurationManager.AppSettings.Get("Port"));
            serverChannel = new TcpServerChannel(port, ip);
            serverChannel.NewHandler += OnNewClient; // for new client
            ClientHandler.MessageRecived += GetMessageFromUser; // for msg from the user
            serverChannel.Start();
        }

        /// <summary>
        /// This function is being called whenever a new client is connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNewClient(object sender, NewClientEventArgs e)
        {
            bool result;
            string answer = _mController.ExecuteCommand((int)CommandEnum.GetConfigCommand, null, out result);
            if (answer != "")
            {
                serverChannel.SendSpecificlly(e.Client, answer);
            }
            lock (thisLock)
            {
                // get the list of logs 
                List<MessageRecievedEventArgs> logMsgsReversed =
                    new List<MessageRecievedEventArgs>(LogService.Instance.LogMsgs);
                // reverse the list
                logMsgsReversed.Reverse();
                // go over the list of logs ans send them to the specific client
                foreach (MessageRecievedEventArgs msg in logMsgsReversed)
                {
                    string[] info = { msg.Status.ToString(), msg.Message };
                    CommandMessage msgC = new CommandMessage((int)CommandEnum.LogCommand, info);
                    serverChannel.SendSpecificlly(e.Client, msgC.ToJson());
                    System.Threading.Thread.Sleep(100);
                }
            }
            _mLogging.Log("New client is connected", MessageTypeEnum.INFO);
            _mLogging.Log("Send config: " + answer, MessageTypeEnum.INFO);
        }

        /// <summary>
        /// This function is being called whenever a new log is being written,
        /// This function is responsible of senting the new log to all the clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void SendLog(Object sender, MessageRecievedEventArgs msg)
        {
            string[] info = { msg.Status.ToString(), msg.Message };
            CommandMessage msgC = new CommandMessage((int)CommandEnum.LogCommand, info);
            serverChannel.SendToAll(msgC.ToJson());
            System.Threading.Thread.Sleep(100);
        }

        /// <summary>
        /// This function is being called when a client wrote a new msg
        /// It transfer the object into CommandMessage and execute the
        /// right command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void GetMessageFromUser(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg == null)
            {
                _mLogging.Log("Can't convert " + info.Data + " to JSON", MessageTypeEnum.FAIL);
                return;
            }
            _mLogging.Log("Got msg from user, Command ID: " + msg.CommandId, MessageTypeEnum.INFO);
            bool result;
            // close command
            if (msg.CommandId == (int)CommandEnum.CloseCommand)
            {
                CommandRecieved?.Invoke(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand,
                    null, msg.Args[0]));
                // send the msg to all the clients
                serverChannel.SendToAll(msg.ToJson());
            }
            else
            {
                string answer = _mController.ExecuteCommand(msg.CommandId, null, out result);
                serverChannel.SendToAll(answer);
            }
        }

        /// <summary>
        /// create new directory handler.
        /// </summary>
        /// <param name="directory"></param>
        private void createHandler(string directory)
        {
            IDirectoryHandler handler = new DirectoryHandler(directory, _mController, _mLogging);
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
            _mLogging.Log(args.Message, MessageTypeEnum.INFO);
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
            _mLogging.Log("Server is being shut down", MessageTypeEnum.INFO);
            string[] directories = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
            foreach (string dir in directories)
            {
                if (Directory.Exists(dir))
                {
                    createCommand((int)CommandEnum.CloseCommand, null, dir);
                }
            }
        }
    }
    #endregion
}