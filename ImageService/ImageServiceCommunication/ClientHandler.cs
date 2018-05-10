using ImageServiceCommunication.Interfaces;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    class ClientHandler : IClientChannel
    {
        public event EventHandler<DataCommandArgs> MessageRecived;

        private readonly ILoggingService _logger;
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private NetworkStream _stream;


        public ClientHandler(TcpClient client, ILoggingService logger)
        {
            _client = client;
            _stream = client.GetStream();
            _logger = logger;
            _reader = new StreamReader(_stream, Encoding.ASCII);
            _writer = new StreamWriter(_stream, Encoding.ASCII);

        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            string commandLine;
            new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        commandLine = _reader.ReadLine();
                        //CommandInfoEventArgs info = JsonConvert.DeserializeObject<CommandInfoEventArgs>(commandLine);

                        MessageRecived?.Invoke(this, new DataCommandArgs(commandLine));
                        _logger.Log("Got command:" + commandLine, MessageTypeEnum.INFO);
                    }


                }
                catch (Exception e)
                {
                    DisposeHandler();
                }
            }).Start();
            return true;
        }

        private void DisposeHandler()
        {
            _reader.Close();
            _writer.Close();
            _client.Close();
        }


        public int Send(string data)
        {
            new Task(() =>
            {
                try
                {
                    _writer.Write(data);
                   // CommandInfoEventArgs info = JsonConvert.DeserializeObject<CommandInfoEventArgs>(data);

                    //DataRecived?.Invoke(this, info);
                    _logger.Log("Send command:" + data, MessageTypeEnum.INFO);
                    
                }
                catch (Exception e)
                {
                    // ignored
                }
            }).Start();
            return 1;
        }
    }
}