
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
        private int _commandID;
        public string[] _args;

        public int CommandId {
            get {
                return this._commandID;
            } set {
                this._commandID = value;
            }
        }      // The Command ID
        public string[] Args {
            get
            {
                return this._args;
            }
            set
            {
                this._args = value;
            }
        }


        public CommandMessage(int id, string[] args)
        {
            _commandID = id;
            _args = args;
        }

        public string ToJson()
        {
            JObject cmdobj = new JObject();
            cmdobj["CommandId"] = this._commandID;
            JArray args = new JArray(this._args);
            cmdobj["Args"] = args;
            return cmdobj.ToString();
        }

        public static CommandMessage FromJson(string str)
        {
            JObject cmdObj = JObject.Parse(str);
            JArray cmdArg = (JArray)cmdObj["Args"];
            string[] args = cmdArg.Select(c => (string)c).ToArray();
            return new CommandMessage((int)cmdObj["CommandId"], args);

        }

    }
}
