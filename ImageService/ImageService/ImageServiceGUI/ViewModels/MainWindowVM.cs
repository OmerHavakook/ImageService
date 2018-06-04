using ImageServiceGUI.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class MainWindowVm : INotifyPropertyChanged
    {
        private IMainWindowModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// c'tor
        /// </summary>
        public MainWindowVm()
        {
            this.model = new MainWindowModel();
            model.PropertyChanged +=
                 delegate (Object sender, PropertyChangedEventArgs e)
                 {
                     NotifyPropertyChanged("VM_" + e.PropertyName);
                 };
            this.CloseCommand = new DelegateCommand<object>(this.OnClose, this.CanClose); // close the gui
        }

        /// <summary>
        /// This method returns when a client can close the gui
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanClose(object arg)
        {
            return true;
        }

        /// <summary>
        /// This method is responsible of the sequnceses of closing the gui
        /// </summary>
        /// <param name="obj"></param>
        private void OnClose(object obj)
        {

        }

        /// <summary>
        /// We used delegate command for closing the gui
        /// </summary>
        public ICommand CloseCommand { get; private set; }

        /// <summary>
        /// This method is being called when a property has changed
        /// </summary>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Property for connect
        /// </summary>
        public bool VM_Connect
        {
            get { return this.model.Connect; }
        }
    }
}
