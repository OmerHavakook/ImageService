namespace ImageServiceCommunication.Interfaces
{
    interface IClientChannel : IChannel
    {
        int Send(string data);
    }
}
