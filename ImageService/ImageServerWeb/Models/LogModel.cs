using ImageServerWeb.Communication;
using ImageServiceCommunication;
using System;
using System.Collections.Generic;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using System.ComponentModel.DataAnnotations;

namespace ImageServerWeb.Models
{
    public class LogModel
    {
        /// <summary>
        /// C'tor
        /// </summary>
        public LogModel()
        {
            TcpClient client = TcpClient.Instance;
            // add event
            client.Channel.MessageRecived += GetMessageFromServer;
            // create the list of msgs
            Messages = new List<MessageRecievedEventArgs>();
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "Messages")]
        public List<MessageRecievedEventArgs> Messages { get; set; }

        /// <summary>
        /// This method is being called whenever a new msg
        /// was sent from the server
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

                    Messages.Insert(0, log);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// This method gets an instance of the TcpClient, In case
        /// the communication is off - it tries to make it
        /// </summary>
        public void Initialize()
        {
            TcpClient client = TcpClient.Instance;
            if (!Connected) { client.Connect(); }
            System.Threading.Thread.Sleep(300);
        }

        /// <summary>
        /// Property
        /// </summary>
        public bool Connected
        {
            get
            {
                return TcpClient.Instance.Connected;
            }
        }
    }
}