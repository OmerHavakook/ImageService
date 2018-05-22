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

        public event PropertyChangedEventHandler PropertyChanged;

        // list of log msgs
        private ObservableCollection<MessageRecievedEventArgs> _mMessages;

        /// <summary>
        /// c'tor
        /// </summary>
        public LogModel()
        {
            Messages = new ObservableCollection<MessageRecievedEventArgs>();
            TcpClient client = TcpClient.Instance;
            // send msg from the user
            client.Channel.MessageRecived += GetMessageFromServer;
        }


        /// <summary>
        /// This method is being called whenever a new msg received from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void GetMessageFromServer(object sender, DataCommandArgs info)
        {
            
                var msg = CommandMessage.FromJson(info.Data);
                if (msg == null)
                {
                    return;
                }
                // if new log is being received
                if (msg.CommandId == (int)CommandEnum.LogCommand)
                {
                    try
                    {
                        // try to parse it
                        MessageRecievedEventArgs log =
                            new MessageRecievedEventArgs((MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), msg.Args[0]), msg.Args[1]);

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                           
                                // add msg to the list
                                _mMessages.Insert(0, log);
                            
                        }));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
        }

        /// <summary>
        /// Property of Messages
        /// </summary>
        public ObservableCollection<MessageRecievedEventArgs> Messages
        {
            get { return this._mMessages; }
            set
            {
                this._mMessages = value;
                this.NotifyPropertyChanged("Messages");
            }
        }

        /// <summary>
        /// Method to make the code shorter (E.G the code we saw at the lecture about
        /// thr robut)
        /// </summary>
        /// <param name="propName"></param> as the property that has changed
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


    }
}
