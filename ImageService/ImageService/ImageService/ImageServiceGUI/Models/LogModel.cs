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

namespace ImageServiceGUI.Models
{
    class LogModel : ILogModel
    {


        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MessageRecievedEventArgs> m_Messages;

        public LogModel()
        {
            Messages = new ObservableCollection<MessageRecievedEventArgs>();
            Messages.Add(new MessageRecievedEventArgs(MessageTypeEnum.INFO, "gfgf"));
            CommandInfo logRequest = new CommandInfo((int)CommandEnum.LogCommand, null);
            TcpClientChannel client = TcpClientChannel.Instance;
            client.MessageReceived += getMessageFromUserL;
           // client.SendCommand(logRequest);
        }

        private void getMessageFromUserL(object sender, CommandInfo info)
        {
            if (info.CommandID == (int)CommandEnum.LogCommand)
            {
                try
                {
                    List<MessageRecievedEventArgs> logs = JsonConvert.DeserializeObject<List<MessageRecievedEventArgs>>(info.Args);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        m_Messages.Clear();
                        foreach(MessageRecievedEventArgs entry in logs)
                        {
                            m_Messages.Add(entry);
                        }

                    }));

                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return this.m_Messages; }
            set
            {
                this.m_Messages = value;
                this.NotifyPropertyChanged("Messages");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


    }
}
