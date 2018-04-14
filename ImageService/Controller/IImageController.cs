using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// This is an interface, using to execute command based on commandID,
    /// arguments and changes the boolean result according to the execution.
    /// </summary>
    interface IImageController
    {
        /// <summary>
        /// this method is responsible of executing a command
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        /// returns a report if the command is a valid command
        string ExecuteCommand(int commandID, string[] args, out bool result);
    }
}
