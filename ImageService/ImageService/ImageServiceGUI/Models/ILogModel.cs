using ImageServiceLogging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    interface ILogModel : INotifyPropertyChanged
    {
         ObservableCollection<LogEntry> Messages { set; get; }
    }
}
