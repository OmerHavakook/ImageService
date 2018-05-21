
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
        // static event!!!
        public static event EventHandler<DataCommandArgs> MessageRecived;

        private readonly TcpClient _client;
        private readonly BinaryReader _reader; // read
        private readonly BinaryWriter _writer; // write
        private readonly NetworkStream _stream;
        private readonly CancellationTokenSource _cancelToken;
        private bool IsConnect;

        public BinaryWriter Writer => this._writer;

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="client"></param>
        public ClientHandler(TcpClient client)
        {
            _client = client;
            _stream = client.GetStream();
            _reader = new BinaryReader(_stream, Encoding.ASCII);
            _writer = new BinaryWriter(_stream, Encoding.ASCII);
            _cancelToken = new CancellationTokenSource();
            this.IsConnect = true;
        }

        /// <summary>
        /// closing the client handler
        /// </summary>
        public void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function starts handling reading msg from the clients
        /// </summary>
        public void Start()
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        string data = _reader.ReadString();
                        if (data != "") // check if not an empty string
                        {
                            // raise the event
                            MessageRecived?.Invoke(this, new DataCommandArgs(data));
                        }
                    }
                    catch (Exception e)
                    {
                        // close reader, writer and client connection
                        DisposeHandler();
                    }
                }

            }, _cancelToken.Token).Start();
        }

        /// <summary>
        /// This function closed the reader, writer and client connection
        /// </summary>
        private void DisposeHandler()
        {
            if (_client.Connected)
                _reader.Close();
            _writer.Close();
            _client.Close();
        }

        /// <summary>
        /// Property for TcpClient member
        /// </summary>
        public TcpClient Client
        {
            get { return _client; }
        }

    }
}