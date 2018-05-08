namespace ImageServiceCommunication.Interfaces
{
    interface IChannel
    {
        void Close();
        bool Start();

    }
}
