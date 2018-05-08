using System;

namespace ImageServiceInfrastructure.Event
{
    public class CommandInfoEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string Args { get; set; }

        public CommandInfoEventArgs(int id, string args)
        {
            CommandID = id;
            Args = args;
        }
    }
}
