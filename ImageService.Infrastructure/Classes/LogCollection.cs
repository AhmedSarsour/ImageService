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
        private List<Log> logs;

        public LogCollection()
        {
            this.logs = new List<Log>();
        }

        public void AddLog(object sender, MessageRecievedEventArgs e)
        {
            this.logs.Add(new Log((int)e.Status, e.Message));
        }

        public void AddLog(Log log)
        {
            this.logs.Add(log);
        }

        public  string ToJSON()
        {
            return JToken.FromObject(logs).ToString();
        }

        public void FromJson(string str)
        {
            JObject j = JObject.Parse(str);

            this.logs = j.ToObject<List<Log>>();
        }


    }
}
