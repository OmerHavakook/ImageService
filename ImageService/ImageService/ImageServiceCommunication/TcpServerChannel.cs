using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    class TcpServerChannel
    {
        private readonly int _port;
        private TcpListener _listener;
        private IClientHandler ch;
        public TcpServerChannel(int port, IClientHandler ch)
        {
            this._port = port;
            this.ch = ch;
        }
        public void Start()
        {
            IPEndPoint ep = new
                IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            _listener = new TcpListener(ep);

            _listener.Start();
            Console.WriteLine("Waiting for connections...");

            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = _listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        ch.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }



        public void Stop()
        {
            _listener.Stop();
        }
    }

  

}
