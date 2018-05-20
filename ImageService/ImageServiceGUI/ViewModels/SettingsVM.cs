using ImageServiceCommunication;
using ImageServiceGUI.Communication;
using ImageServiceGUI.Models;
using ImageServiceInfrastructure.Enums;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class SettingsVM : INotifyPropertyChanged
    {
        private ISettingsModel model;
        public SettingsVM()
        {
            /// CREATE OUTSIDE!!!!!!!!!!!!!!!
            this.model = new SettingsModel();
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            
        }

        private void OnRemove(object obj)
        {
            string[] args = { _selectedItem };
            CommandMessage msg = new CommandMessage((int)CommandEnum.CloseCommand, args);
            TcpClient instance = TcpClient.Instance;
            instance.Channel.Write(msg.ToJson());

        }

        private bool CanRemove(object obj)
        {
            if (_selectedItem != null)
            {
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RemoveCommand { get; private set; }


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        public string VM_OutputDirectory => model.OutputDirectory;

        public string VM_SourceName => model.SourceName;

        public string VM_LogName => model.LogName;

        public int VM_ThumbnailSize => model.ThumbnailSize;

        public ObservableCollection<string> VM_Handlers => model.Handlers;

        private string _selectedItem;
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
