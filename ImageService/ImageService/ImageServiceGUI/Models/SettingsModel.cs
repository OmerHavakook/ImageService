using ImageServiceCommunication;
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
            instance.Channel.MessageRecived += GetMessageFromUserS;
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

        public void GetMessageFromUserS(object sender, DataCommandArgs info)
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
            this._mOutputDirectory = settings[0];
            this._mSourceName = settings[1];
            this._mLogName = settings[2];
            this._mThumbnailSize = Int32.Parse(settings[3]);
            int i = 4;
            this.Handlers.Clear();
            try
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    while (settings[i] != null)
                    {
                        Handlers.Add(settings[i]);
                        i++;
                    }
                }));

            } catch (Exception e)
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
