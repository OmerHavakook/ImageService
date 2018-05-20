using System;

namespace ImageServiceInfrastructure.Event
{
    public class DataCommandArgs : EventArgs
    {
        public DataCommandArgs(string data)
        {
            this.Data = data;
        }

        public string Data { get; }
    }
}
