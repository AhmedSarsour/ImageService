using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Classes
{
    public class MessageToClient : Jsonable
    {
       //Which message we send - for example getconfig, new log, handler we just closed..
        public int TypeMessage{get;private set;}
        //The message itself
        public string Content { get; private set; }

        public MessageToClient(int type, string content)
        {
            this.TypeMessage = type;
            this.Content = content;
        }
        //Empty constructor for case we want to get the properties from json
        public MessageToClient()
        {

        }
        public void FromJson(string str)
        {
            JObject messageObj = JObject.Parse(str);
            this.TypeMessage = (int)messageObj["TypeMessage"];

            this.Content = (string)messageObj["Content"];
        }

        public string ToJSON()
        {
            JObject messageObj = new JObject();
            messageObj["TypeMessage"] = TypeMessage;
            messageObj["Content"] = Content;
            return messageObj.ToString();
        
        }
    }
}
