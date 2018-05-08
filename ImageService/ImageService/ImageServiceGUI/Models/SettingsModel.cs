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
        delegate TOut ParamsFunc<TIn, TOut>(params TIn[] args);

        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<int, System.Action> functions;

        //public ObservableCollection<string> Handlers { get; set; }

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
        //    this.functions = new Dictionary<int, System.Action>()
      //  {
        //    { (int)CommandEnum.GetConfigCommand, initializeConfig }
       // };
            CommandInfo initializeConfig = new CommandInfo((int)CommandEnum.GetConfigCommand, null);
            TcpClientChannel client = TcpClientChannel.Instance;
            client.SendCommand(initializeConfig);
           
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

        public getMessageFromUser(object sender, CommandInfo info)
        {
            functions[info.CommandID].initializeConfig(info);
        }

        public void initializeConfig(CommandInfo info)
        {
            TcpClientChannel client = TcpClientChannel.Instance;
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
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void removeHandler(CommandInfo info)

    }

}
