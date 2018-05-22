
using ImageServiceInfrastructure.Event;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class TcpServerChannel
    {
        private readonly int _port;
        private readonly string _ip;
        private TcpListener _listener;
        public static List<ClientHandler> Clients;
        public event EventHandler<NewClientEventArgs> NewHandler;
        private bool _running;
        private static readonly Mutex WriterMut = new Mutex();



        public TcpServerChannel(int port, string ip)
        {
            this._port = port;
            _ip = ip;
            _running = true;
            Clients = new List<ClientHandler>();
        }

        public void Close()
        {
            _running = false;
            _listener.Stop();
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), _port);
            this._listener = new TcpListener(ep);

            this._listener.Start();

            Task task = new Task(() =>
            {

                while (_running)
                {
                    try
                    {
                        TcpClient client = this._listener.AcceptTcpClient();
                        ClientHandler newclient = new ClientHandler(client);
                        newclient.Start();
                        Clients.Add(newclient);
                        System.Threading.Thread.Sleep(500);
                        NewHandler?.Invoke(this, new NewClientEventArgs(client));
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

        public void SendToAll(string data)
        {
            Task task = new Task(() =>
            {
                foreach (ClientHandler ch in Clients)
                {
                    try
                    {
                        WriterMut.WaitOne();
                        ch.Writer.Write(data);
                        WriterMut.ReleaseMutex();
                    }
                    catch (Exception e)
                    {
                        Clients.Remove(ch);
                        ch.Client.Close();
                    }
                }
                Console.WriteLine("write " + data);
            });
            task.Start();
        }

        public void SendSpecificlly(TcpClient clientSpecific, string data)
        {
            Task task = new Task(() =>
            {
                foreach (ClientHandler ch in Clients)
                {
                    if (ch.Client.Equals(clientSpecific))
                    {
                        try
                        {
                            WriterMut.WaitOne();
                            ch.Writer.Write(data);
                            WriterMut.ReleaseMutex();
                        }
                        catch (Exception e)
                        {
                            Clients.Remove(ch);
                            ch.Client.Close();
                        }
                    }

                }
            });
            task.Start();
        }
    }
}