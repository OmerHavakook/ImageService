using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    class CloseCommand : ICommand
    {
        // this method will never be called
        public string Execute(string[] args, out bool result)
        {
            result = true;
            return "The close command has occurred successfully";
        }
    }
}
