using System;

namespace ImageServiceInfrastructure.Event
{
    public class DataCommandArgs : EventArgs
    {
        private string _data;

        public DataCommandArgs(string data)
        {
            this._data = data;
        }
    }
}
