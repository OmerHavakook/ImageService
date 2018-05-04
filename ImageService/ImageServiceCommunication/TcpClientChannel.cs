using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ImageServiceCommunication
{
    class TcpClientChannel
    {

        private TcpClient _client;

        private static TcpClientChannel _clientInstance;


        public static TcpClientChannel Instance
        {
            get
            {
                if (_clientInstance == null)
                {
                    _clientInstance = new TcpClientChannel();

                }
                return _clientInstance;
            }
        }

        public void Connect()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                _client = new TcpClient();
                _client.Connect(ep);
                Console.WriteLine("You are connected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public JObject ReciveJObject(int command)
        {
            using (NetworkStream stream = _client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                var json = reader.ReadBytes(int.MaxValue);
                var jobj = JObject.Parse(json.ToString());
                return jobj;
            }
        }
        public void SendCommand(int command)
        {
            using (NetworkStream stream = _client.GetStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                writer.Write(command);
                Console.WriteLine($"Send {command} to Server");

            }
        }



    }
}
