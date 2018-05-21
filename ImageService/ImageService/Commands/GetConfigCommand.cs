using ImageServiceCommunication;
using ImageServiceInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                string[] handlers = ConfigurationManager.AppSettings.Get("Handler").Split(';');
                string[] info = new string[handlers.Length + 4];
                info[0] = ConfigurationManager.AppSettings.Get("OutputDir");
                info[1] = ConfigurationManager.AppSettings.Get("SourceName");
                info[2] = ConfigurationManager.AppSettings.Get("LogName");
                info[3] = ConfigurationManager.AppSettings.Get("ThumbnailSize");
                int i = 4;
                foreach (string handler in handlers)
                {
                    if (handler != "")
                    {
                        info[i] = handler;
                        i++;
                    }
                    
                }
                result = true;
                return new CommandMessage((int)CommandEnum.GetConfigCommand, info).ToJson();
            } catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }

        public GetConfigCommand()
        {
            


        }
    }
}
