using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Models
{
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        // ADD ALL THE COMMUNICATION WITH THE SERVER!!!!!!!!!!!!

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // ADD C'TOR THAT COMMUNICATE WITH THE SERVER!!!!!!

        private string m_OutputDirectory = "aaa";
        public string OutputDirectory
        {
            get { return this.m_OutputDirectory; }
            set
            {
                m_OutputDirectory = value;
                NotifyPropertyChanged("OutputDirectory");
            }
        }

        private string m_SourceName = "bbb";
        public string SourceName
        {
            get { return this.m_SourceName; }
            set
            {
                m_SourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        private string m_LogName = "ccc";
        public string LogName
        {
            get { return this.m_LogName; }
            set
            {
                m_LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        private int m_ThumbnailSize = 120;
        public int ThumbnailSize
        {
            get { return this.m_ThumbnailSize; }
            set
            {
                m_ThumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public ObservableCollection<string> Handlers { get; set; }

    }
}
