
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    public class CommandMessage
    {
        private int _commandId;
        public string[] _args;

        /// <summary>
        /// Property to CommandID(int)
        /// </summary>
        public int CommandId
        {
            get
            {
                return this._commandId;
            }
            set
            {
                this._commandId = value;
            }
        }      // The Command ID

        /// <summary>
        /// Property to Args(string[])
        /// </summary>
        public string[] Args
        {
            get
            {
                return this._args;
            }
            set
            {
                this._args = value;
            }
        }

        /// <summary>
        /// c'tor
        /// </summary>
        /// <param name="id"></param> for commandID
        /// <param name="args"></param> for args
        public CommandMessage(int id, string[] args)
        {
            _commandId = id;
            _args = args;
        }

        /// <summary>
        /// This function converts an obj from typr CommandMessage into
        /// string using Json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            try
            {
                JObject cmdobj = new JObject();
                cmdobj["CommandId"] = this._commandId;
                JArray args = new JArray(this._args);
                cmdobj["Args"] = args;
                return cmdobj.ToString();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return "";
            }
        }

        /// <summary>
        /// This is a static method which converts from string into a
        /// CommandMessage type ojb (using Json)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static CommandMessage FromJson(string str)
        {
            try
            {
                JObject cmdObj = JObject.Parse(str);
                JArray cmdArg = (JArray)cmdObj["Args"];
                string[] args = cmdArg.Select(c => (string)c).ToArray();
                return new CommandMessage((int)cmdObj["CommandId"], args);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return null;
            }

        }
    }
}
