using ImageServiceCommunication.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;

namespace ImageServiceCommunication
{
    public class TcpClientChannel : IClientChannel
    {
        private int _port;
        private string _ip;
        private TcpClient _client;
        private IClientChannel _cHandler;

        public TcpClientChannel(int port, string ip)
        {
            this._port = port;
            this._ip = ip;
            _cHandler = null;
        }


        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), _port);
                _client = new TcpClient();
                _client.Connect(ep);
                _cHandler.Start();
                Console.WriteLine("You are connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return true;
        }

        public int Send(string data)
        {
            return _cHandler.Send(data);
        }
    }
}
