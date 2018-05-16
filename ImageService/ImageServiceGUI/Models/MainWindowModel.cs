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

        public MainWindowModel()
        {
            ////////////// TRY TO MAKE AN INSTANCE!!!!!!!!!!!!!!!!!!
            m_Connect = false;

        }
        public bool Connect {
            get { return m_Connect; }
            set
            {
                m_Connect = value;
                NotifyPropertyChanged("Connect");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
