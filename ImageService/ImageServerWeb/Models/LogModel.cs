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

        public LogModel()
        {
            TcpClient client = TcpClient.Instance;
            client.Channel.MessageRecived += GetMessageFromServer;
            Messages = new List<MessageRecievedEventArgs>();
        }

        [Required]
        [Display(Name = "Messages")]
        public List<MessageRecievedEventArgs> Messages { get; set; }

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

        public void Initialize()
        {
            TcpClient client = TcpClient.Instance;
            if (!Connected) { client.Connect(); }
            System.Threading.Thread.Sleep(300);
        }
        public bool Connected
        {
            get
            {
                return TcpClient.Instance.Connected;
            }
        }

    }
}