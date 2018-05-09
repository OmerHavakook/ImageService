using ImageServiceInfrastructure.Event;
using ImageServiceLogging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    interface ILogModel : INotifyPropertyChanged
    {
         ObservableCollection<MessageRecievedEventArgs> Messages { set; get; }
    }
}
