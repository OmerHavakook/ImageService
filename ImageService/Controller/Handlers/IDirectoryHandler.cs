using ImageService.Modal.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    interface IDirectoryHandler
    {
        /// <summary>
        /// The Event That Notifies that the Directory is being closed
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath"></param>
        void StartHandleDirectory(string dirPath);

        /// <summary>
        /// // The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
    }
}