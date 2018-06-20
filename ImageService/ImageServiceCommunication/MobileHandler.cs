using ImageServiceCommunication.Interfaces;
using ImageServiceInfrastructure.Event;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class MobileHandler : IClientHandler
    {
        #region Members
        private TcpClient m_client;         // The Client Instance
        private NetworkStream m_stream;     // The Stream To Close The Stream
        private BinaryReader m_reader;            // The Reader          

        private CancellationTokenSource m_cancelToken;          // The Cancelation Token
        #endregion

        public MobileHandler(TcpClient client)
        {
            m_client = client;
            m_stream = client.GetStream();
            m_reader = new BinaryReader(m_stream, Encoding.ASCII);
            m_cancelToken = new CancellationTokenSource();
        }
        public static event EventHandler<MobileCommandArgs> DataRecieved;

        // The Function Closes The Handler
        public void Close()
        {
            m_cancelToken.Cancel();         // Canceling the Recieve
        }

        // Starting to Recieve Data
        public void Start()
        {
            new Task(() =>
            {
                try
                {
                    while (true)
                    {
                        byte[] bytes = new byte[4096];


                        //read the photo name
                        int bytesTransfered = m_stream.Read(bytes, 0, bytes.Length);
                        string picName = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);

                        //read the bytes of pic
                        int resultRead = m_stream.Read(bytes, 0, bytes.Length);
                        int temp = resultRead;
                        byte[] current;
                        int numRead = resultRead;
                        //while we didnt read all the pic length
                        while (numRead != 0)
                        {
                            current = new byte[4096];
                            numRead = m_stream.Read(current, 0, current.Length);
                            transferBytes(bytes, current, temp);
                            temp += numRead;
                        }

                        DataRecieved?.Invoke(this, new MobileCommandArgs(picName, bytes));



                        /**
                         * int bytesTransfered = m_stream.Read(bytes, 0, bytes.Length);
                        string piclen = Encoding.ASCII.GetString(bytes, 0, bytesTransfered);  



                        byte[] picture = GetPhoto();

                        DataRecieved?.Invoke(this, new MobileCommandArgs(piclen,picture));
                         */
                    }
                }
                catch (Exception e)
                {
                    Close();
                }
            }, m_cancelToken.Token).Start();

        }

        public void transferBytes(byte[] origin, byte[] toCopy, int start)
        {
            for (int i = start; i < origin.Length; i++)
            {
                origin[i] = toCopy[i - start];
            }
        }
    }
}

