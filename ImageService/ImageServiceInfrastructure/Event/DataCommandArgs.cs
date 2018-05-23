using System;

namespace ImageServiceInfrastructure.Event
{
    public class DataCommandArgs : EventArgs
    {
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="data"></param>
        public DataCommandArgs(string data)
        {
            this.Data = data;
        }

        public string Data { get; } // string
    }
}
