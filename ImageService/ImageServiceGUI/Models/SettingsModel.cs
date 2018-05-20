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
    class SettingsModel : ISettingsModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            TcpClient client = TcpClient.Instance;
            client.Channel.MessageRecived += GetMessageFromUser;
        }


        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


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

        public string SelectedItem
        {
            get { return SelectedItem; }
            set { SelectedItem = value; }
        }

        public void GetMessageFromUser(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg.CommandId == (int)CommandEnum.GetConfigCommand)
            {
                InitializeConfig(msg.Args);
            }
            else if (msg.CommandId == (int)CommandEnum.CloseCommand)
            {
                removeHandler(msg.Args);
            }
        }

        public void InitializeConfig(string[] settings)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    this.OutputDirectory = settings[0];
                    this.SourceName = settings[1];
                    this.LogName = settings[2];
                    this.ThumbnailSize = int.Parse(settings[3]);
                    for (int i = 4; i < settings.Length; i++)
                    {
                        Handlers.Add(settings[i]);
                    }
                }));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }

        }

        public void removeHandler(string[] info)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    string handlerToRemove = info[0];
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
