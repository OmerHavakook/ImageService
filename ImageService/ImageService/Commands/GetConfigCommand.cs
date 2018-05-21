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

        /// <summary>
        /// In this command we read the config settings from the cofigurationManager,
        /// than we created a CommandMessage obj filled with the data.
        /// We transfer this object into a string and returns it.
        /// </summary>
        /// <param name="args"></param> 
        /// <param name="result"></param> the bool
        /// <returns></returns>
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
                foreach (string handler in handlers) // fill with all the handlers
                {
                    if (handler != "") // if the handler is not an empty string
                    {
                        info[i] = handler;
                        i++;
                    }

                }
                result = true;
                return new CommandMessage((int)CommandEnum.GetConfigCommand, info).ToJson();
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
