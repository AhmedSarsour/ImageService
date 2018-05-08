using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ImageService.Infrastructure.Classes
{
    public class LogCollection: Jsonable
    {
        public List<Log> Logs;

        public LogCollection()

        {
            this.Logs = new List<Log>();
        }

        public void AddLog(object sender, MessageRecievedEventArgs e)
        {
            this.Logs.Add(new Log((int)e.Status, e.Message));
        }

        public void AddLog(Log log)
        {
            this.Logs.Add(log);
        }

        public  string ToJSON()
        {
            //JObject configObj = new JObject();
            ////Converting the list to json
            //configObj["Logs"] = JToken.FromObject(Logs);
            //return configObj.ToString();
            return JsonConvert.SerializeObject(Logs, Formatting.Indented);
        }

        public void FromJson(string str)
        {
            //    JObject configObj = null;
            //try
            //{
            //  configObj = JObject.Parse(str);
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("LAMAA "+ e.Data);
            //}

            List<Log> products = JsonConvert.DeserializeObject<List<Log>>(str);
            //Logs = (configObj["Logs"]).ToObject<List<Log>>();
            this.Logs = products;
        }


    }
}
