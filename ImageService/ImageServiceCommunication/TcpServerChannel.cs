using ImageServiceCommunication.Interfaces;
using ImageServiceInfrastructure.Enums;
using ImageServiceLogging.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ImageServiceInfrastructure.Event;
using System.Collections.Generic;
using System.IO;

namespace ImageServiceCommunication
{
    public class TcpServerChannel : IChannel
    {
        private readonly int _port;
        private readonly string _ip;
        private readonly ILoggingService _logger;
        private TcpListener _listener;
        private List<TcpClient> _clients;

        public TcpServerChannel(int port, string ip)
        {
            this._port = port;
            _ip = ip;

            _logger = new LoggingService();
            _clients = new List<TcpClient>();
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
                        _clients.Add(client);
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

        public event EventHandler<DataCommandArgs> MessageRecived;

        public void SendToAll(string data)
        {
            foreach (TcpClient client in _clients)
            {
                new Task(() =>
                {
                   using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(data);
                }
                }).Start();
            }
    }



}