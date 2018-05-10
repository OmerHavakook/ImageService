using System;

namespace ImageServiceInfrastructure.Event
{
    public class DataCommandArgs : EventArgs
    {
        private string _data;

        public DataCommandArgs(string data)
        {
            this.Data = data;
        }

        public string Data { get => _data; set => _data = value; }
    }
}
