using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /*private ObservableCollection<LogEntry> m_Messages;
         public ObservableCollection<LogEntry> Messages
         {
             get { return this.m_Messages; }
             set => throw new NotImplementedException();
             }
 
         }*/
    }
}
