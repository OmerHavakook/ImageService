using ImageServiceCommunication.Interfaces;
using ImageServiceInfrastructure.Event;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageServiceInfrastructure.Enums;

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
                    byte[] picBytes = new byte[4096];
                    while (true)
                    {
                    
                        int readerCounter = m_stream.Read(picBytes, 0, picBytes.Length);
                        if (readerCounter == 0 || picBytes == null) // no read or error
                            break;
                        // make length
                        string picLength = Encoding.ASCII.GetString(picBytes, 0, readerCounter);
                        if (picLength == "End\n")
                        {
                            break;
                        }
                        // create an array of bytes at the size of the pic
                        picBytes = new byte[int.Parse(picLength)];
                        // read from buffer
                        readerCounter = m_stream.Read(picBytes, 0, picBytes.Length);
                        // get image name
                        string picName = Encoding.ASCII.GetString(picBytes, 0, readerCounter);
                        // read bytes
                        int resultRead = m_stream.Read(picBytes, 0, picBytes.Length);
                        int temp = resultRead;
                        byte[] current;
                        // if more bytes should be readed than continue reading
                        while (temp < picBytes.Length)
                        {
                            current = new byte[int.Parse(picLength)];
                            readerCounter = m_stream.Read(current, 0, current.Length);
                            addBytes(picBytes, current, temp);
                            temp += readerCounter;
                        }
                        DataRecieved?.Invoke(this, new MobileCommandArgs(picName, picBytes));
                    }
                }
                catch (Exception e)
                {
                    Close();
                }
            }, m_cancelToken.Token).Start();
        }

        /// <summary>
        /// This function transfer bytes between buffers
        /// </summary>
        /// <param name="deliverTo"></param> array of byte to transfer to
        /// <param name="deliverFrom"></param> array of byte to transfer from
        /// <param name="start"></param> index
        public void addBytes(byte[] deliverTo, byte[] deliverFrom, int start)
        {
            for (int i = start; i < deliverTo.Length; i++)
            {
                deliverTo[i] = deliverFrom[i - start];
            }
        }
    }
}

