using ImageServiceCommunication;
using ImageServiceGUI.Communication;
using ImageServiceGUI.Models;
using ImageServiceInfrastructure.Enums;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class SettingsVM : INotifyPropertyChanged
    {
        private ISettingsModel model;

        /// <summary>
        /// c'tor
        /// </summary>
        public SettingsVM()
        {
            /// CREATE OUTSIDE!!!!!!!!!!!!!!!
            this.model = new SettingsModel();
   

            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            // used for removing handlers
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
        }

        /// <summary>
        /// This method is responsible of sending the request to send an handler
        /// to the server
        /// </summary>
        /// <param name="obj"></param>
        private void OnRemove(object obj)
        {
            string[] args = { _selectedItem };
            CommandMessage msg = new CommandMessage((int)CommandEnum.CloseCommand, args);
            TcpClient instance = TcpClient.Instance;
            instance.Channel.Write(msg.ToJson());
        }

        /// <summary>
        /// This method is responsible of deciding when an handler can be removed
        /// </summary>
        /// <param name="obj"></param>
        private bool CanRemove(object obj)
        {
            if (_selectedItem != null)
            {
                return true;
            }
            return false;
        }

        public void ChangeSelected()
        {
            this.SelectedItem = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// We used delegate command in order to delete an handler
        /// </summary>
        public ICommand RemoveCommand { get; private set; }

        /// <summary>
        /// This method is being called when a peoperty has changed 
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Property for outputDirectory
        /// </summary>
        public string VM_OutputDirectory => model.OutputDirectory;

        /// <summary>
        /// Property for SourceName
        /// </summary>
        public string VM_SourceName => model.SourceName;

        /// <summary>
        /// Property for LogName
        /// </summary>
        public string VM_LogName => model.LogName;

        /// <summary>
        /// Property for ThumbnailSize
        /// </summary>
        public int? VM_ThumbnailSize => model.ThumbnailSize;

        /// <summary>
        /// Property for Handlers
        /// </summary>
        public ObservableCollection<string> VM_Handlers => model.Handlers;

        private string _selectedItem;

        /// <summary>
        /// Property for SelectedItem
        /// </summary>
        public string SelectedItem
        {
            set
            {
                this._selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this._selectedItem; }
        }

        

    }
}
