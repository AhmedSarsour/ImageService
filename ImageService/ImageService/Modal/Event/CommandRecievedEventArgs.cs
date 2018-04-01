using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class CommandRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// the CommandRecieved event's args:
        /// the ID of the command.
        /// the args.
        /// the request's directory's path.
        /// </summary>
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        /// <summary>
        /// the Constructor, setting the values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        /// <param name="path"></param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
