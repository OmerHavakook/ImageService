using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageServiceCommunication;
using ImageServiceInfrastructure.Event;
using ImageServiceInfrastructure.Enums;
using System.Collections.Generic;
using ImageServiceGUI.Convertors;
using System;
using Newtonsoft.Json;
using System.Windows;

namespace ImageServiceGUI.Models
{
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            CommandInfo initializeConfigC = new CommandInfo((int)CommandEnum.GetConfigCommand, null);
            TcpClientChannel client = TcpClientChannel.Instance;
            client.MessageReceived += getMessageFromUserS;
           // client.SendCommand(initializeConfigC);
           
        }

        // ADD ALL THE COMMUNICATION WITH THE SERVER!!!!!!!!!!!!

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // ADD C'TOR THAT COMMUNICATE WITH THE SERVER!!!!!!

        private string m_OutputDirectory;
        public string OutputDirectory
        {
            get { return this.m_OutputDirectory; }
            set
            {
                m_OutputDirectory = value;
                NotifyPropertyChanged("OutputDirectory");
            }
        }

        private string m_SourceName;
        public string SourceName
        {
            get { return this.m_SourceName; }
            set
            {
                m_SourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        private string m_LogName;
        public string LogName
        {
            get { return this.m_LogName; }
            set
            {
                m_LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        private int m_ThumbnailSize;
        public int ThumbnailSize
        {
            get { return this.m_ThumbnailSize; }
            set
            {
                m_ThumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public ObservableCollection<string> Handlers { get; set; }
        public object SettingData { get; private set; }

        public void getMessageFromUserS(object sender, CommandInfo info)
        {
            if (info.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                initializeConfig(info);
            } else if (info.CommandID == (int)CommandEnum.CloseCommand)
            {
                removeHandler(info);
            }
        }

        public void initializeConfig(CommandInfo info)
        {
            try
            {
                SettingsData settings = JsonConvert.DeserializeObject<SettingsData>(info.Args);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.m_OutputDirectory = settings.OutputDir;
                    this.m_LogName = settings.LogName;
                    this.m_SourceName = settings.SourceName;
                    this.m_ThumbnailSize = settings.ThumbnailSize;
                    this.Handlers.Clear();
                    foreach (string handlerInLoop in settings.Handlers)
                    {
                        Handlers.Add(handlerInLoop);
                    }

                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void removeHandler(CommandInfo info)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    string handlerToRemove = info.Args;
                    Handlers.Remove(handlerToRemove);
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }

}
