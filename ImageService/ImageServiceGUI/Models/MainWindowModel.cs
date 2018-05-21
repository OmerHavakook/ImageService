using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    class MainWindowModel : IMainWindowModel
    {

        private bool m_Connect;

        /// <summary>
        /// c'tor
        /// </summary>
        public MainWindowModel()
        {
            TcpClient client = TcpClient.Instance;
            m_Connect = client.Connected;
        }

        /// <summary>
        /// Property for connect
        /// </summary>
        public bool Connect {
            get { return m_Connect; }
            set
            {
                m_Connect = value;
                NotifyPropertyChanged("Connect");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// like NotifyPropertyChanged in LogModel
        /// </summary>
        /// <param name="propName"></param> - the name of the property that has changed
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
