using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageServiceInfrastructure.Enums;

namespace ImageServiceInfrastructure.Event
{
    public class NewClientEventArgs:EventArgs
    {
        private TcpClient _client;
        
        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="client"></param> TcpClient
        public NewClientEventArgs(TcpClient client)
        {
            _client = client;
        }

        public TcpClient Client => this._client;
    }
}
