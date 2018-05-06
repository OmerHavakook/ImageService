using ImageServiceGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    class SettingsVM : INotifyPropertyChanged
    {
        private ISettingsModel model;
        public SettingsVM()
        {
            this.model = new SettingsModel();
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
                };

        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string VM_OutputDirectory
        {
            get { return model.OutputDirectory; }
        }

        public string VM_SourceName
        {
            get { return model.SourceName; }
        }

        public string VM_LogName
        {
            get { return model.LogName; }
        }

        public int VM_ThumbnailSize
        {
            get { return model.ThumbnailSize; }
        }



    }
}
