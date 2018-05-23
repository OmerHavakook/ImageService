using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OutputDirectory { set; get; }
        string SourceName { set; get; }
        string LogName { set; get; }
        int? ThumbnailSize { set; get; }
        string SelectedItem { set; get; }
        ObservableCollection<string> Handlers { set; get; }
    }
}
