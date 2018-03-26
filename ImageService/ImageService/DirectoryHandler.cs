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
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;


        public DirectoryHandler(IImageController m_controller, ILoggingService m_logging)
        {
            this.m_controller = m_controller;
            this.m_logging = m_logging;
            m_dirWatcher = new FileSystemWatcher();
            m_dirWatcher.Created += M_dirWatcher_Created; 
            m_dirWatcher.Filter = "*.jpg ,*.png ,*.gif ,*.bmp";
        }

        public void M_dirWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string[] arg = { e.FullPath };
            m_controller.ExecuteCommand(1, arg, out bool res);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartHandleDirectory(string dirPath)
        {
           
        }
    }
}
