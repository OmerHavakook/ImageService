using ImageServiceCommunication;

namespace ImageServiceGUI.Communication
{
    public class Connection
    {
        private static Connection _instance;
        public bool IsConnect { get; set; }

        public TcpClientChannel Channel { get; set; }

        public static Connection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Connection();
                    _instance.Channel.Start();
                }

                return _instance;
            }
        }

        public Connection()
        {
            Channel = new TcpClientChannel(8000, "127.0.0.1");
        }
    }
}
