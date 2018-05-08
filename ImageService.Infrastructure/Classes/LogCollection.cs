using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
using Newtonsoft.Json.Linq;

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
            return JToken.FromObject(Logs).ToString();
        }

        public void FromJson(string str)
        {
            JObject j = JObject.Parse(str);

            this.Logs = j.ToObject<List<Log>>();
        }


    }
}
