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
    /// <summary>
    /// LogCollection class.
    /// </summary>
    public class LogCollection: Jsonable
    {
        //list of logs.
        public List<Log> Logs;
        /// <summary>
        /// LogCollection's constructor.
        /// </summary>
        public LogCollection()
        {
            this.Logs = new List<Log>();
        }
        
        /// <summary>
        /// adding a log to the list of logs as a message arguments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddLog(object sender, MessageRecievedEventArgs e)
        {
            this.Logs.Add(new Log((int)e.Status, e.Message));
        }

        /// <summary>
        /// adding a log to the list of logs.
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(Log log)
        {
            this.Logs.Add(log);
        }
        /// <summary>
        /// converting the logss into json object.
        /// </summary>
        /// <returns></returns>
        public  string ToJSON()
        {
            return JsonConvert.SerializeObject(Logs, Formatting.Indented);
        }
        /// <summary>
        /// getting a list of logs from the json object.
        /// </summary>
        /// <param name="str"></param>
        public void FromJson(string str)
        {
            List<Log> products = JsonConvert.DeserializeObject<List<Log>>(str);
            this.Logs = products;
        }
    }
}
