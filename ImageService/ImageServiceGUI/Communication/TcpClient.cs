using System;
using System.Configuration;
using ImageServiceCommunication;

namespace ImageServiceGUI.Communication
{
    /// <summary>
    /// We used the TcpClient as a singleton in order to get the same
    /// TcpClient the all time (which means than for one gui client we will
    /// have only one communication channel!)
    /// </summary>
    public class TcpClient
    {
        private static TcpClient _instance;
        private bool connected;

        /// <summary>
        /// Property for channel
        /// </summary>
        public TcpClientChannel Channel { get; set; }

        /// <summary>
        /// static method, at the first call it creates a new TcpClient,
        /// and since that call it always returns the same TcpClient(instance)
        /// </summary>
        public static TcpClient Instance
        {
            get
            {
                if (_instance == null) // first call to Instance
                {
                    _instance = new TcpClient();
                }

                return _instance;
            }
        }

        /// <summary>
        /// c'tor
        /// </summary>
        public TcpClient()
        {
            string ip = ConfigurationManager.AppSettings.Get("Ip");
            int port = Int32.Parse(ConfigurationManager.AppSettings.Get("Port"));
            Channel = new TcpClientChannel(port, ip); // create channel
            Connected = Channel.Start();
        }

        /// <summary>
        /// Property for connection
        /// </summary>
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