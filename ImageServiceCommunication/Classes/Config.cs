using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;
using Newtonsoft.Json.Linq;

namespace ImageServiceCommunication.Classes
{
    public class Config:Jsonable
    {
        public List<string> Handlers { get; set; }
        //The Output directory.
        public string OutputDir { get; set; }
        //The source name in the event log
        public string SourceName { get; set; }
        //The log name
        public string LogName { get; set; }
        //The thumbnail picture size
        public int ThumbnailSize { get; set; }

        public Config()
        {

        }

        public string ToJSON()
        {
            JObject configObj = new JObject();
            //Converting the list to json
            configObj["Handlers"] = JToken.FromObject(Handlers);
            configObj["OutputDir"] = OutputDir;
            configObj["SourceName"] = SourceName;
            configObj["LogName"] = LogName;
            configObj["ThumbnailSize"] = ThumbnailSize;

            return configObj.ToString();

        }

        public void FromJson(string str)
        {
            JObject configObj = JObject.Parse(str);
            Handlers = (configObj["Handlers"]).ToObject<List<string>>();
            LogName = (string)configObj["LogName"];
   
            SourceName = (string)configObj["SourceName"];
            OutputDir = (string)configObj["OutputDir"];
            ThumbnailSize = (int)configObj["ThumbnailSize"];

        }
    }
}
