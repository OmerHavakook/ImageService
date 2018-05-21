using ImageServiceCommunication;
using ImageServiceGUI.Communication;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ImageServiceGUI.Models
{
    class LogModel : ILogModel
    {
        private Object thisLock = new Object();

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MessageRecievedEventArgs> _mMessages;

        public LogModel()
        {
            Messages = new ObservableCollection<MessageRecievedEventArgs>();
            TcpClient client = TcpClient.Instance;
            client.Channel.MessageRecived += GetMessageFromUser;
        }

        private void GetMessageFromUser(object sender, DataCommandArgs info)
        {
            
                var msg = CommandMessage.FromJson(info.Data);
                if (msg.CommandId == (int)CommandEnum.LogCommand)
                {
                    try
                    {
                        MessageRecievedEventArgs log =
                            new MessageRecievedEventArgs((MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), msg.Args[0]), msg.Args[1]);

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            lock (thisLock)
                            {
                                _mMessages.Insert(0, log);
                            }
                        }));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            
        }

        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return this._mMessages; }
            set
            {
                this._mMessages = value;
                this.NotifyPropertyChanged("Messages");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


    }
}
