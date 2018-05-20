using System;
using System.Configuration;
using ImageServiceCommunication;

namespace ImageServiceGUI.Communication
{
    public class TcpClient
    {
        private static TcpClient _instance;

        private bool connected;

        public TcpClientChannel Channel { get; set; }

        public static TcpClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TcpClient();
                }

                return _instance;
            }
        }

        public TcpClient()
        {
            string ip = ConfigurationManager.AppSettings.Get("Ip");
            int port = Int32.Parse(ConfigurationManager.AppSettings.Get("Port"));
            Channel = new TcpClientChannel(port, ip);
            Connected = Channel.Start();
        }

        public bool Connected
        {
            get { return this.connected; }
            set
            {
                connected = value;
            }
        }
    }
}