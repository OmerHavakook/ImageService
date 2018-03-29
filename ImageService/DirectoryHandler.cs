using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
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

        public DirectoryHandler(string path, IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            m_logging = logger;
            m_dirWatcher = new FileSystemWatcher();
            StartHandleDirectory(path);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.Args != null && e.RequestDirPath.Equals(m_path))
            {
                if (e.CommandID == 1)
                {
                    handleAddingFile(e);
                }
                else
                {
                    handleClose();
                }
            }
        }

        public void handleClose() {

        }

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


        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            m_dirWatcher.Path = m_path;
            m_dirWatcher.NotifyFilter = NotifyFilters.Attributes;
            m_dirWatcher.Filter = "*.*";
            m_dirWatcher.Changed += new FileSystemEventHandler(checkEvent);
            m_dirWatcher.EnableRaisingEvents = true;

        }

        private void checkEvent(object source, FileSystemEventArgs e)
        {
            String[] args = { e.FullPath };
            string ending = Path.GetExtension(e.FullPath);
            string[] endings = { ".bmp", ".gif", ".png", ".jpg" };
            if (endings.Contains(ending))
            {
                CommandRecievedEventArgs eventArg = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand,
                    args, this.m_path);
                this.OnCommandRecieved(this, eventArg);
            }
        }
    }
}
