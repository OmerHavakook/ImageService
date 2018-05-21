using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageServiceInfrastructure.Enums;

namespace ImageService.Controller
{
    /// <summary>
    /// This class implements the IImageController interface
    /// </summary>
    class ImageController : IImageController
    {
        private IImageServiceModal m_modal;
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// constructor that creates a dictionary of the commands
        /// </summary>
        /// <param name="m_modal"></param>
        public ImageController(IImageServiceModal m_modal)
        {
            this.m_modal = m_modal;
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand ,new NewFileCommand(m_modal) },
                {(int)CommandEnum.GetConfigCommand,new GetConfigCommand() },
                {(int)CommandEnum.CloseCommand, new CloseCommand() }
            };
        }

        /// <summary>
        /// In this implementation there is a call for the command to execute
        /// using the dictionary
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns>
        /// returns a report if the command is a valid command
        /// </returns>
        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            if (commands.ContainsKey(commandID))
            {
                result = true;
                // execute the command
                return commands[commandID].Execute(args, out result);
            }
            result = false;
            return "Not such a command";
        }
    }
}