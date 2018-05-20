using ImageServiceInfrastructure.Event;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        private string _ip;
        private int _port;

        private bool _isConnect;

        public TcpClientChannel(int port, string ip)
        {
            _port = port;
            _ip = ip;
        }


        public bool Connect => this._isConnect;


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

        public void Write(string str)
        {

            try
            {
                Console.WriteLine("write to server...");
                _writer.Write(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        private void Read()
        {
            new Task(() =>
            {
                while (_isConnect)
                {
                    try
                    {
                        string msg = _reader.ReadString();
                        Console.WriteLine("reading from server: " + msg);
                        MessageRecived?.Invoke(this, new DataCommandArgs(msg));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }).Start();
        }


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