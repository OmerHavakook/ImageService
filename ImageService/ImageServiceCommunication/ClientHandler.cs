
using ImageServiceInfrastructure.Event;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class ClientHandler
    {
        public static event EventHandler<DataCommandArgs> MessageRecived;


        private readonly TcpClient _client;
        private readonly BinaryReader _reader;
        private readonly BinaryWriter _writer;
        private readonly NetworkStream _stream;
        private readonly CancellationTokenSource _cancelToken;
        private bool IsConnect;

        public BinaryWriter Writer => this._writer;

        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _reader = new BinaryReader(_stream, Encoding.ASCII);
            _writer = new BinaryWriter(_stream, Encoding.ASCII);
            _cancelToken = new CancellationTokenSource();
            this.IsConnect = true;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        string data = _reader.ReadString();
                        if (data != "")
                        {
                            MessageRecived?.Invoke(this, new DataCommandArgs(data));
                        }

                    }
                    catch (Exception e)
                    {

                        DisposeHandler();
                    }
                }

            }, _cancelToken.Token).Start();
        }

        private void DisposeHandler()
        {
            if (_client.Connected)
                _reader.Close();
            _writer.Close();
            _client.Close();
        }

        public TcpClient Client
        {
            get { return _client; }
        }
        
    }
}