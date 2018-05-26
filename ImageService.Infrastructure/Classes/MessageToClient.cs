using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Classes
{
    /// <summary>
    /// MessageToClient Class.
    /// </summary>
    public class MessageToClient : Jsonable
    {
       //Which message we send - for example getconfig, new log, handler we just closed..
        public int TypeMessage{get;private set;}
        //The message itself
        public string Content { get; private set; }

        public bool AllClients { get;  set; }
        /// <summary>
        /// the constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="allClients"></param>
        public MessageToClient(int type, string content, bool allClients)
        {
            this.TypeMessage = type;
            this.Content = content;
            this.AllClients = allClients;
        }
        //Empty constructor for case we want to get the properties from json
        public MessageToClient()
        {

        }
        /// <summary>
        /// getting the information from the json.
        /// </summary>
        /// <param name="str"></param>
        public void FromJson(string str)
        {
            JObject messageObj = JObject.Parse(str);
            this.TypeMessage = (int)messageObj["TypeMessage"];

            this.Content = (string)messageObj["Content"];

            this.AllClients = (bool)messageObj["AllClients"];
        }
        /// <summary>
        /// converting the information into json object.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            JObject messageObj = new JObject();
            messageObj["TypeMessage"] = TypeMessage;
            messageObj["Content"] = Content;
            messageObj["AllClients"] = AllClients;
            return messageObj.ToString();
        }
    }
}
