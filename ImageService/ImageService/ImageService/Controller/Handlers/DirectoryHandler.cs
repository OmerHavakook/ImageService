using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging.Logging;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{

    class DirectoryHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory

        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="path"></param>
        /// <param name="controller"></param>
        /// <param name="logger"></param>
        public DirectoryHandler(string path, IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            m_logging = logger;
            m_dirWatcher = new FileSystemWatcher();
            StartHandleDirectory(path);
        }

        /// <summary>
        /// the event method that activates the controller 
        /// and sends message to the logging if the fail or sucsess.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(m_path))
            {
                if (e.CommandID == (int)CommandEnum.NewFileCommand)
                {
                    m_logging.Log("New file command received for path: " + m_path, MessageTypeEnum.INFO);
                    handleAddingFile(e);
                }
                else if (e.CommandID == (int)CommandEnum.CloseCommand)
                {
                    m_logging.Log("Close command received for path: " + m_path, MessageTypeEnum.INFO);
                    handleClose();
                }
            }
        }

        /// <summary>
        /// when the service is closed it sends a massage to all the handlers 
        /// and close them.
        /// </summary>
        public void handleClose()
        {
            string msg;
            try
            {
                m_dirWatcher.EnableRaisingEvents = false; // stop raising events
                msg = "Handler at path " + m_path + " was closed";
                DirectoryCloseEventArgs dirArg = new DirectoryCloseEventArgs(m_path, msg);
                DirectoryClose?.Invoke(this, dirArg);
            }
            catch (Exception e)
            {
                m_logging.Log(e.ToString(), MessageTypeEnum.FAIL);
            }
            finally
            {
                m_dirWatcher.Created -= new FileSystemEventHandler(checkEvent);
                bool result;
                string[] args = { m_path };
                // remove handler from the config
                m_controller.ExecuteCommand((int)CommandEnum.CloseCommand, args, out result);
            }
        }

        /// <summary>
        /// this function handle adding new file to the dirctory.
        /// by create new task and move the image using image modal.
        /// </summary>
        /// <param name="e"></param>
        public void handleAddingFile(CommandRecievedEventArgs e)
        {
            Task addingTask = new Task(() =>
            {
                bool result;
                String msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);

                if (result)
                {
                    m_logging.Log(msg, MessageTypeEnum.INFO);
                }
                else
                {
                    m_logging.Log(msg, MessageTypeEnum.FAIL);
                }
            });
            addingTask.Start();
        }

        /// <summary>
        /// this function works on when the server start to handle directory.
        /// </summary>
        /// <param name="dirPath"></param>
        public void StartHandleDirectory(string dirPath)
        {
            string msg = "Start handle directory at path: " + dirPath;
            m_logging.Log(msg, MessageTypeEnum.INFO);
            this.m_path = dirPath;
            m_dirWatcher.Path = m_path;
            m_dirWatcher.Filter = "*";
            m_dirWatcher.Created += new FileSystemEventHandler(checkEvent);
            m_dirWatcher.EnableRaisingEvents = true;
        }

        ///<summary>
        /// this function is the file watcher event handler.
        /// </summary>
        ///<param name="source"></param>
        ///<param name="e"></param>
        private void checkEvent(object source, FileSystemEventArgs e)
        {
            Thread.Sleep(50); //wait for image being prefectly in directory.
            String[] args = { e.FullPath };
            string ending = Path.GetExtension(e.FullPath);
            string[] endings = { ".bmp", ".gif", ".png", ".jpg" };
            if (endings.Contains(ending.ToLower()))
            {
                CommandRecievedEventArgs eventArg = new CommandRecievedEventArgs(
                    (int)CommandEnum.NewFileCommand, args, this.m_path);
                this.OnCommandRecieved(this, eventArg);
            }
        }
    }
}
