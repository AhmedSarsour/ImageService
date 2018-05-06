using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;

namespace ImageService.Infrastructure.Classes
{
    public class Log: Jsonable
    {

        public string Type { get; private set; }
        public string Message { get; private set; }

        public Log(int type, string message)
        {
            switch (type) {
                case (int)MessageTypeEnum.INFO:
                    Type = "INFO";
                    break;
                case (int)MessageTypeEnum.WARNING:
                    Type = "WARRNING";
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

        public string ToJSON()
        {
            JObject configObj = new JObject();
            //Converting the list to json
            configObj["Type"] = Type;
            configObj["Message"] = Message;


            return configObj.ToString();

        }

        public void FromJson(string str)
        {
            JObject configObj = JObject.Parse(str);
            Type = (string)configObj["Type"];

            Message = (string)configObj["Message"];

        }

        public static string ListToJSON(List<Log> logs)
        {
            return JToken.FromObject(logs).ToString();
        }

        public static List<Log> JSONToList(string str)
        {
            JObject j = JObject.Parse(str);

            return j.ToObject<List<Log>>();
        }
    }
}
