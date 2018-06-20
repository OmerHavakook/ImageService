using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceInfrastructure.Event;

namespace ImageServiceCommunication.Interfaces
{
    interface IClientHandler
    {
        void Start();
        void Close();
    }
}
