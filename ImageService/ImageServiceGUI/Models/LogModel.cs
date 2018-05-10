using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageServiceLogging;
using ImageServiceCommunication;
using ImageServiceInfrastructure.Event;
using ImageServiceInfrastructure.Enums;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Models
{
    class LogModel : ILogModel
    {


        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MessageRecievedEventArgs> _mMessages;

        public LogModel()
        {
            Messages = new ObservableCollection<MessageRecievedEventArgs>();
            Connection instance = Connection.Instance;
            instance.Channel.MessageRecived += GetMessageFromUserL;
            //CommandInfoEventArgs logRequest = new CommandInfoEventArgs((int)CommandEnum.LogCommand, null);
            /*TcpClientChannel client = TcpClientChannel.Instance;
            client.MessageReceived += getMessageFromUserL;*/
            // client.SendCommand(logRequest);
        }

        private void GetMessageFromUserL(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg.CommandId == (int)CommandEnum.LogCommand)
            {
               
                try
                {
                    MessageRecievedEventArgs log = 
                        new MessageRecievedEventArgs((MessageTypeEnum) Enum.Parse(typeof(MessageTypeEnum), msg.Args[0]), msg.Args[1]);


                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        _mMessages.Add(log);


                    }));

                } catch (Exception e)
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
