
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
        
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ip"></param>
        public TcpServerChannel(int port, string ip)
        {
            this._port = port;
            _ip = ip;
            _running = true;
            Clients = new List<ClientHandler>();
        }

        /// <summary>
        /// closing the server channel
        /// </summary>
        public void Close()
        {
            _running = false;
            _listener.Stop();
        }

        /// <summary>
        /// Starting the server channel, In this method we waited for new
        /// clients and accept them in a new thread
        /// </summary>
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
                        Clients.Add(newclient); // add new clientHandler to the list
                        System.Threading.Thread.Sleep(500);
                        // invoke this event in order to get logs and settings
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

        /// <summary>
        /// In this method we send a msg to all the server clients by going 
        /// over the list of client handlers and send the msg to each one
        /// </summary>
        /// <param name="data"></param>
        public void SendToAll(string data)
        {
            Task task = new Task(() =>
            {
                foreach (ClientHandler ch in Clients)
                {
                    if (ch.IsConnect)
                    {
                        try
                        {
                            WriterMut.WaitOne(); // lock
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

        /// <summary>
        /// This method sends a msg to a specific client (just one and not
        /// to all of the client)
        /// </summary>
        /// <param name="clientSpecific"></param>
        /// <param name="data"></param>
        public void SendSpecificlly(TcpClient clientSpecific, string data)
        {
            Task task = new Task(() =>
            {
                // find the client in the list
                foreach (ClientHandler ch in Clients)
                {
                    if (ch.Client.Equals(clientSpecific))
                    {
                        try
                        {
                            WriterMut.WaitOne(); // lock
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