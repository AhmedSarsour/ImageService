using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ImageService.Infrastructure.Classes
{
    /// <summary>
    /// Log's class.
    /// </summary>
    public class Log: Jsonable
    {
        //type of the message.
        public string Type { get; private set; }
        //the message itself.
        public string Message { get; private set; }

        /// <summary>
        /// the log's constructor.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public Log(int type, string message)
        {
            switch (type) {
                case (int)MessageTypeEnum.INFO:
                    Type = "INFO";
                    break;
                case (int)MessageTypeEnum.WARNING:
                    Type = "WARNING";
                    break;
                case (int)MessageTypeEnum.FAIL:
                    Type = "FAIL";
                    break;
                default:
                    Type = "Wrong type";
                    break;
            }
            this.Message = message;
        }
        [JsonConstructor]
        public Log(string type, string message)
        {
            this.Type = type;
            this.Message = message;
        }

        /// <summary>
        /// converting the log into a json object.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            JObject configObj = new JObject();
            //Converting the list to json
            configObj["Type"] = Type;
            configObj["Message"] = Message;
            return configObj.ToString();

        }
        /// <summary>
        /// extracting the type and message from the json object.
        /// </summary>
        /// <param name="str"></param>
        public void FromJson(string str)
        {
            JObject configObj = JObject.Parse(str);
            Type = (string)configObj["Type"];
            Message = (string)configObj["Message"];
        }
    }
}
