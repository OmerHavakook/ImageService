using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceInfrastructure.Event
{
    public class CommandInfo : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string Args { get; set; }

        public CommandInfo(int id, string args)
        {
            CommandID = id;
            Args = args;
        }
    }
}
