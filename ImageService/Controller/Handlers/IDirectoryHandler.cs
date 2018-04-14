
using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// This is an interface responsible of listening to directories
    /// </summary>
    interface IDirectoryHandler
    {
        /// <summary>
        /// The Event That Notifies that the Directory is being closed 
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        void StartHandleDirectory(string dirPath);             // The Function Recieves the directory to Handle
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command
    }
}