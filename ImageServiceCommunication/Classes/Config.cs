using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication.Classes
{
    public class Config
    {
        public List<string> Handlers { get; private set; }
        //The Output directory.
        public string OutPutDir { get; private set; }
        //The source name in the event log
        public string SourceName { get; private set; }
        //The log name
        public string LogName { get; private set; }
        //The thumbnail picture size
        public int ThumbnailSize { get; private set; }
    }
}
