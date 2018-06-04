using ImageServiceInfrastructure.Event;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class TcpClientChannel
    {
        public event EventHandler<DataCommandArgs> MessageRecived;

        private TcpClient _client;
        private NetworkStream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private readonly string _ip;
        private readonly int _port;
        private bool _isConnect;
        private static readonly Mutex WriterMut = new Mutex();

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ip"></param>
        public TcpClientChannel(int port, string ip)
        {
            _port = port;
            _ip = ip;
        }

        /// <summary>
        /// Property for Connect
        /// </summary>
        public bool Connect => this._isConnect;

        /// <summary>
        /// Starting the client channel, In this function I created reader
        /// and writer in order to write and reat throught the client stream.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(_ip), _port);
                this._client = new TcpClient();
                _client.Connect(ep);
                this._stream = _client.GetStream();
                this._reader = new BinaryReader(_stream);
                this._writer = new BinaryWriter(_stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _isConnect = false;
                return false;
            }

            _isConnect = true;
            Read();
            return true;
        }

        /// <summary>
        /// In this method we wrote to the server, we used mutex in writing
        /// </summary>
        /// <param name="str"></param>
        public void Write(string str)
        {
            try
            {
                WriterMut.WaitOne(); // lock
                _writer.Write(str);
                WriterMut.ReleaseMutex();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Close();
            }
        }

        /// <summary>
        /// In this funcion we read from the server in a thread as long
        /// as the client is connected
        /// </summary>
        private void Read()
        {
            new Task(() =>
            {
                while (_isConnect)
                {
                    try
                    {
                        string msg = _reader.ReadString();
                        MessageRecived?.Invoke(this, new DataCommandArgs(msg));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Close();
                    }
                }
            }).Start();
        }

        /// <summary>
        /// This function close the reader, writer and the stream.
        /// Moreover, it close the client itself (TcpClient)
        /// </summary>
        public void Close()
        {
            _isConnect = false;
            this._writer.Close();
            this._reader.Close();
            this._stream.Close();
            this._client.Close();
        }
    }
}