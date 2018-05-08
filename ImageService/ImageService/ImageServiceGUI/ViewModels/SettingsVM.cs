using ImageServiceGUI.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.RemoveCommand = new DelegateCommand<object>(this.OnRemove,this.CanRemove);
        }

        private void OnRemove(object obj)
        {
            Console.WriteLine("Trying to remove");
        }

        private bool CanRemove(object obj)
        {
            if (selectedItem != null)
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

        public ObservableCollection<string> VM_Handlers
        {
            get { return model.Handlers; }
        }

        private string selectedItem;
        public string SelectedItem
        {
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get { return this.selectedItem; }
        }

    }
}
