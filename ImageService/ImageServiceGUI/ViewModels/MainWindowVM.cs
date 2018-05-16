using ImageServiceCommunication;
using ImageServiceGUI.Communication;
using ImageServiceGUI.Models;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModels
{
    class MainWindowVM : INotifyPropertyChanged
    {
        private IMainWindowModel model;

        public event PropertyChangedEventHandler PropertyChanged;


        public MainWindowVM()
        {
            this.model = new MainWindowModel();
            model.PropertyChanged +=
                 delegate (Object sender, PropertyChangedEventArgs e)
                 {
                     NotifyPropertyChanged("VM_" + e.PropertyName);
                 };
            this.CloseCommand = new DelegateCommand<object>(this.OnClose, this.CanClose);
        }

        private bool CanClose(object arg)
        {
            return true;
        }

        private void OnClose(object obj)
        {

            Console.WriteLine("BKLA");
            ;
            ;
            ;
            // REMOVE TO EVENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


            /*CommandMessage msg = new CommandMessage(5, null); // FIX!!!!!!!!!!!!!
            Connection instance = Connection.Instance;
            instance.Channel.Send(msg.ToJson());*/


            // SEND MSG
            // CLOSE COMMUNITCATION
        }

        public ICommand CloseCommand { get; private set; }

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public bool VM_Connect
        {
            get { return this.model.Connect; }
        }
        
    }
}
