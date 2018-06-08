using ImageServerWeb.Communication;
using ImageServiceCommunication;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Configuration;

namespace ImageServerWeb.Models
{
    public class ConfigModel
    {
        public ConfigModel()
        {
            TcpClient client = TcpClient.Instance;
            client.Channel.MessageRecived += GetMessageFromServer;
            Handlers = new List<string>();
            OutputDirectory = null;
        }

        [Required]
        [Display(Name = "IsRemoved")]
        public bool IsRemoved { get; set; }

        public void Initialize()
        {
            TcpClient client = TcpClient.Instance;
            if (!Connected) { client.Connect();}
            System.Threading.Thread.Sleep(300); 
        }

        [Required]
        [Display(Name = "SelectedItem")]
        public string SelectedItem { get; set; }

        public bool Connected
        {
            get
            {
                return TcpClient.Instance.Connected;
            }
        }

        [Required]
        [Display(Name = "OutputDirectory")]
        /// <summary>
        /// Property for _mOutputDirectory
        /// </summary>
        public string OutputDirectory { get; set; }

        [Required]
        [Display(Name = "SourceName")]
        /// <summary>
        /// Property for _mSourceName
        /// </summary>
        public string SourceName { get; set; }

        [Required]
        [Display(Name = "LogName")]
        /// <summary>
        /// Property for _mLogName
        /// </summary>
        public string LogName { get; set; }

        [Required]
        [Display(Name = "ThumbnailSize")]
        /// <summary>
        /// Property for _mThumbnailSize
        /// </summary>
        public int? ThumbnailSize { get; set; }

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

        public void removeHandler(string handler)
        {
            this.Handlers.Remove(handler);
           IsRemoved = true;
        }

        public void InitializeConfig(string[] settings)
        {
            try
            {
                OutputDirectory = settings[0];
                SourceName = settings[1];
                LogName = settings[2];
                ThumbnailSize = int.Parse(settings[3]);

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

        public void OnRemove()
        {
            IsRemoved = false;
            string[] args = { SelectedItem };
            CommandMessage msg = new CommandMessage((int)CommandEnum.CloseCommand, args);
            TcpClient instance = TcpClient.Instance;
            instance.Channel.Write(msg.ToJson());
            while (!IsRemoved)
            { }
        }

        [Required]
        [Display(Name = "Handlers")]
        public List<string> Handlers { get; set; }
    }
}