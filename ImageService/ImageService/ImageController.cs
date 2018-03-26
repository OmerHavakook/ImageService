using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
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
                {1,new NewFileCommand(m_modal) }
                // For Now will contain NEW_FILE_COMMAND
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool result)
        {
            if (commands.ContainsKey(commandID))
            {
                return commands[commandID].Execute(args, out result);
            }
            result = false;
            return "Not such a command";
        }
    }
}