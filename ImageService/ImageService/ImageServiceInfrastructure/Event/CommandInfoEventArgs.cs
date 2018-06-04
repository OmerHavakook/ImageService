using System;

namespace ImageServiceInfrastructure.Event
{
    public class CommandInfoEventArgs : EventArgs
    {
        /// <summary>
        /// The command ID
        /// </summary>
        public int CommandId { get; set; }      // The Command ID

        /// <summary>
        /// Args for command
        /// </summary>
        public string Args { get; set; }

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="id"></param> commandID
        /// <param name="args"></param> arguments
        public CommandInfoEventArgs(int id, string args)
        {
            CommandId = id;
            Args = args;
        }
    }
}
