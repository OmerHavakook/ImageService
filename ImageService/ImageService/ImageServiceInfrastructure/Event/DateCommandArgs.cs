using System;

namespace ImageServiceCommunication
{
    public class DateCommandArgs : EventArgs
    {
        private string _data;

        public DateCommandArgs(string data)
        {
            this._data = data;
        }
    }
}