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



        /// <summary>
        /// c'tor
        /// </summary>
        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            TcpClient client = TcpClient.Instance;
            client.Channel.MessageRecived += GetMessageFromServer;
            this.ThumbnailSize = null;
        }

        /// <summary>
        /// Method to make the code shorter (E.G the code we saw at the lecture about
        /// thr robut)
        /// </summary>
        /// <param name="propName"></param> as the property that has changed
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private string _mOutputDirectory;

        /// <summary>
        /// Property for _mOutputDirectory
        /// </summary>
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

        /// <summary>
        /// Property for _mSourceName
        /// </summary>
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

        /// <summary>
        /// Property for _mLogName
        /// </summary>
        public string LogName
        {
            get { return this._mLogName; }
            set
            {
                _mLogName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        private int? _mThumbnailSize;

        /// <summary>
        /// Property for _mThumbnailSize
        /// </summary>
        public int? ThumbnailSize
        {
            get { return this._mThumbnailSize; }
            set
            {
                _mThumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }

        /// <summary>
        /// Property for handlers
        /// </summary>
        public ObservableCollection<string> Handlers { get; set; }
        public object SettingData { get; private set; }

        /// <summary>
        /// Property for SelectedItem
        /// </summary>
        public string SelectedItem
        {
            get { return SelectedItem; }
            set { SelectedItem = value; }
        }

        /// <summary>
        /// This method is being called whenever a message received from the server
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
                removeHandler(msg.Args);
            }
        }

        /// <summary>
        /// This method initialize the config involves members
        /// </summary>
        /// <param name="settings"></param>
        public void InitializeConfig(string[] settings)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    OutputDirectory = settings[0];
                    SourceName = settings[1];
                    LogName = settings[2];
                    ThumbnailSize = int.Parse(settings[3]);

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

        /// <summary>
        /// This method removes one handler from the list of handlers
        /// </summary>
        /// <param name="info"></param>
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
