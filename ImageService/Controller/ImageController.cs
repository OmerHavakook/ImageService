using ImageService.Commands;
using ImageService.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal m_modal)
        {
            this.m_modal = m_modal;
            commands = new Dictionary<int, ICommand>()
            {
                {(int)CommandEnum.NewFileCommand ,new NewFileCommand(m_modal) },
                //{(int)CommandEnum.CloseCommand, new CloseCommand() }
                // For Now will contain NEW_FILE_COMMAND
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            if (commands.ContainsKey(commandID))
            {
                result = true;
                return commands[commandID].Execute(args, out result);
            }
            result = false;
            return "Not such a command";
        }
    }
}