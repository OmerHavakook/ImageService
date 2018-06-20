using ImageServiceInfrastructure.Enums;
using ImageServiceLogging.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class TCPMobileServer
    {
        private readonly int _port;
        private readonly string _ip;
        private readonly ILoggingService _mLogging;
        private TcpListener _listener;
        private bool _running;

        public TCPMobileServer(int port, string ip, ILoggingService mLogging)
        {
            _port = port;
            _ip = ip;
            _mLogging = mLogging;
            _running = true;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, _port);
            this._listener = new TcpListener(ep);

            this._listener.Start();

            Task task = new Task(() =>
            {

                while (_running)
                {
                    try
                    {
                        _mLogging.Log("waiting for the connection", MessageTypeEnum.INFO);
                        TcpClient client = this._listener.AcceptTcpClient();
                        _mLogging.Log("new connection", MessageTypeEnum.INFO);
                        MobileHandler newclient = new MobileHandler(client);
                        newclient.Start();
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.Message);
                        Close();
                    }
                }
            });
            task.Start();
        }

        private void Close()
        {
            _running = false;
            _listener.Stop();
        }
    }

}

