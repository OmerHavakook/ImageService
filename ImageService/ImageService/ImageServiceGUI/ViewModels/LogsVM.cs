using ImageServiceGUI.Models;
using ImageServiceInfrastructure.Event;
using ImageServiceLogging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModels
{
    public class LogsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ILogModel model;

        /// <summary>
        /// c'tor
        /// </summary>
        public LogsVM()
        {
            this.model = new LogModel();
            model.PropertyChanged += 
                delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        /// <summary>
        /// This method is being called when a property has changed
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Property for the msgs
        /// </summary>
        public ObservableCollection<MessageRecievedEventArgs> VM_Messages
        {
            get { return model.Messages; }
        }
    }
}
