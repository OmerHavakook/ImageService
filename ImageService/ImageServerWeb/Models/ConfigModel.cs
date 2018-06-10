using ImageServerWeb.Communication;
using ImageServiceCommunication;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace ImageServerWeb.Models
{
    public class ConfigModel
    {
        /// <summary>
        /// c'tor
        /// </summary>
        public ConfigModel()
        {
            // creating an instance of the communication channel
            TcpClient client = TcpClient.Instance;
            // adding the event of the notifications
            client.Channel.MessageRecived += GetMessageFromServer;
            Handlers = new List<string>(); // creating a list of handlers
            OutputDirectory = null; // initialize the OutputDirectory to null
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "IsRemoved")]
        public bool IsRemoved { get; set; }

        /// <summary>
        /// Trying to to make a communication with the service
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
        [Required]
        [Display(Name = "SelectedItem")]
        public string SelectedItem { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        public bool Connected => TcpClient.Instance.Connected;

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "OutputDirectory")]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "ThumbnailSize")]
        public int? ThumbnailSize { get; set; }

        /// <summary>
        /// This method is being invoken whenever the server sends
        /// the TcpChannel msgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        public void GetMessageFromServer(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg == null)
            {
                return;
            }
            if (msg.CommandId == (int)CommandEnum.GetConfigCommand) // config
            {
                InitializeConfig(msg.Args);
            }
            else if (msg.CommandId == (int)CommandEnum.CloseCommand) // close
            {
                removeHandler(msg.Args[0]);
            }
        }

        /// <summary>
        /// This method removes a handler from the list of handlers
        /// Is being called when the server asks to.
        /// </summary>
        /// <param name="handler"></param>
        public void removeHandler(string handler)
        {
            this.Handlers.Remove(handler);
            IsRemoved = true; // change bool
        }

        /// <summary>
        /// This method changes the config info
        /// </summary>
        /// <param name="settings"></param> data that was sent from
        /// the user
        public void InitializeConfig(string[] settings)
        {
            try
            {
                OutputDirectory = settings[0];
                SourceName = settings[1];
                LogName = settings[2];
                ThumbnailSize = int.Parse(settings[3]);
                // adding handlers
                for (int i = 4; i < settings.Length; i++)
                {
                    if (settings[i] != null)
                        Handlers.Add(settings[i]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// This mehod is being called whenever the user asked
        /// to remove an handler
        /// </summary>
        public void OnRemove()
        {
            IsRemoved = false;
            string[] args = { SelectedItem };
            CommandMessage msg = new CommandMessage((int)CommandEnum.CloseCommand, args);
            TcpClient instance = TcpClient.Instance;
            instance.Channel.Write(msg.ToJson()); // notify the server
            SpinWait.SpinUntil(() => IsRemoved, 4000);// wait until the service updates the data
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "Handlers")]
        public List<string> Handlers { get; set; }
    }
}