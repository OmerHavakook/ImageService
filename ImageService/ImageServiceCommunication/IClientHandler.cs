using System.Net.Sockets;

namespace ImageServiceCommunication
{
    internal interface IClientHandler
    {
        void HandleClient(TcpClient client);
    }
}