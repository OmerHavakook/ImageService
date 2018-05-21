using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class CloseCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                Configuration m_Configuration = System.Configuration.ConfigurationManager.
                    OpenExeConfiguration(ConfigurationUserLevel.None);
                // get all the handlers from the config settings
                string[] handlers = (ConfigurationManager.AppSettings.Get("Handler").Split(';'));
                m_Configuration.AppSettings.Settings.Remove("Handler");
                StringBuilder allHandlers = new StringBuilder();
                // search for an handler to close in the app config list of handlers
                // goes over the list of handlers, if it's not the asked one, than add
                // it to the new list of handlers
                foreach (string handlerInArray in handlers)
                {
                    if (string.Compare(args[0], handlerInArray) != 0 &&
                        string.Compare("", handlerInArray) != 0)
                    {
                        allHandlers.Append(handlerInArray);
                        allHandlers.Append(";");
                    }
                }
                ConfigurationManager.AppSettings.Set("Handler", allHandlers.ToString());
                result = true;
                return "Fixed config file";
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
        }
    }
}
