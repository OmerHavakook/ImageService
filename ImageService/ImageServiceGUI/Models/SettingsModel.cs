using ImageServiceGUI.Communication;
using ImageServiceGUI.Convertors;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ImageServiceGUI.Models
{
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            //CommandInfoEventArgs initializeConfigC = new CommandInfoEventArgs((int)CommandEnum.GetConfigCommand, null);
            Connection instance = Connection.Instance;
            instance.Channel.MessageReceived += GetMessageFromUserS;
            //instance.Channel.Send(initializeConfigC);
            /*TcpClientChannel client = TcpClientChannel.Instance;
            client.MessageReceived += getMessageFromUserS;*/
            // client.SendCommand(initializeConfigC);

        }

        // ADD ALL THE COMMUNICATION WITH THE SERVER!!!!!!!!!!!!

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        // ADD C'TOR THAT COMMUNICATE WITH THE SERVER!!!!!!

        private string _mOutputDirectory;
        public string OutputDirectory
        {
            get { return this._mOutputDirectory; }
            set
            {
                _mOutputDirectory = value;
                NotifyPropertyChanged("OutputDirectory");
            }
        }

        private string _mSourceName;
        public string SourceName
        {
            get { return this._mSourceName; }
            set
            {
                _mSourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }

        private string _mLogName;
        public string LogName
        {
            get { return this._mLogName; }
            set
            {
                _mLogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        private int _mThumbnailSize;
        public int ThumbnailSize
        {
            get { return this._mThumbnailSize; }
            set
            {
                _mThumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public ObservableCollection<string> Handlers { get; set; }
        public object SettingData { get; private set; }

        public string selectedItem;
        public string SelectedItem
        {
            get { return this.SelectedItem; }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void GetMessageFromUserS(object sender, CommandInfoEventArgs info)
        {
            if (info.CommandId == (int)CommandEnum.GetConfigCommand)
            {
                InitializeConfig(info);
            }
            else if (info.CommandId == (int)CommandEnum.CloseCommand)
            {
                removeHandler(info);
            }
        }

        public void InitializeConfig(CommandInfoEventArgs info)
        {
            try
            {
                SettingsData settings = JsonConvert.DeserializeObject<SettingsData>(info.Args);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this._mOutputDirectory = settings.OutputDir;
                    this._mLogName = settings.LogName;
                    this._mSourceName = settings.SourceName;
                    this._mThumbnailSize = settings.ThumbnailSize;
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

        public void removeHandler(CommandInfoEventArgs info)
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
