using ImageServiceInfrastructure.Event;
using System;

namespace ImageServiceCommunication.Interfaces
{
    interface IChannel
    {
        void Close();
        bool Start();

        event EventHandler<DataCommandArgs> MessageRecived;
    }
}
