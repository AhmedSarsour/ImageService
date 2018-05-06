using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand: ICommand
    {
        public List<Log> Logs { get; set; }
        public LogCommand(ref List<Log> logs)
        {
            Logs = logs;
        }
        public string Execute(string[] args, out bool result)
        {
            //Our convention - the path will be stored on args[0]
            result = true;
            //The file path will be stored in args[0] so we will run the add file.
            string json = Log.ListToJSON(Logs);
            return json;
       
        }
    }
}
