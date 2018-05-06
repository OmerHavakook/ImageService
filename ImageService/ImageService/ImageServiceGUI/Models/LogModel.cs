using System.Collections.ObjectModel;
using System.ComponentModel;
using
using ImageServiceLogging;

namespace ImageServiceGUI.Models
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<LogEntry> m_Messages;
        public ObservableCollection<LogEntry> Messages
        {
            get { return this.m_Messages; }
            set
            {
                this.m_Messages = value;
                this.NotifyPropertyChanged("Messages");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
