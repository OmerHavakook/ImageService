using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Models
{
    interface ISettingsModel : INotifyPropertyChanged
    {
        string OutputDirectory { set; get; }
        string SourceName { set; get; }
        string LogName { set; get; }
        int ThumbnailSize { set; get; }
        ObservableCollection<string> Handlers { set; get; }
    }
}
