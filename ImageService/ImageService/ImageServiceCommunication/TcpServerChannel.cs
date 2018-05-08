using ImageServiceCommunication.Interfaces;
using ImageServiceInfrastructure.Enums;
using ImageServiceLogging.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    class TcpServerChannel : IChannel
    {
        private readonly int _port;
        private readonly string _ip;
        private readonly ILoggingService _logger;
        private TcpListener _listener;

        public TcpServerChannel(int port, string ip)
        {
            this._port = port;
            _ip = ip;

            _logger = new LoggingService();
        }

        public void Close()
        {
            ///SEND TO ALL CLIENTSSSS//
            _listener.Stop();
        }

        public bool Start()
        {
            IPEndPoint ep = new
                IPEndPoint(IPAddress.Parse(_ip), _port);
            _listener = new TcpListener(ep);

            _listener.Start();
            _logger.Log("Waiting for connections...", MessageTypeEnum.INFO);

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = _listener.AcceptTcpClient();
                        _logger.Log("Got new connection", MessageTypeEnum.INFO);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
            return true;
        }



    }



}
