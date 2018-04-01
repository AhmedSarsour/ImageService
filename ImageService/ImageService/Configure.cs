using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService
{
    class Configure
    {
        //The folders we want to handle.
        public List<string> Handlers { get; private set; }
        //The Output directory.
        public string OutPutDir { get; private set; }
        //The source name in the event log
        public string SourceName { get; private set; }
        //The log name
        public string LogName { get; private set; }
        //The thumbnail picture size
        public int ThumbnailSize { get; private set; }
        public Configure(string path)
        {
            //Making the list of handlers folders by splitting it by ; character.
            this.Handlers = new List<string>(ConfigurationManager.AppSettings["Handler"].Split(new char[] { ';' }));
            this.OutPutDir = ConfigurationManager.AppSettings.Get("OutPutDir");
            this.SourceName = ConfigurationManager.AppSettings["SourceName"];
            this.LogName = ConfigurationManager.AppSettings["LogName"];
            this.ThumbnailSize = int.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }

    }
}
