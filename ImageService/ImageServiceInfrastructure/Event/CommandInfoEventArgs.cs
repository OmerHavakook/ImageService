using System;

namespace ImageServiceInfrastructure.Event
{
    public class CommandInfoEventArgs : EventArgs
    {
        public int CommandId { get; set; }      // The Command ID
        public string Args { get; set; }

        public CommandInfoEventArgs(int id, string args)
        {
            CommandId = id;
            Args = args;
        }
    }
}
