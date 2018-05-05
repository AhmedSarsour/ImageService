using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Event
{
    /// <summary>
    /// The CommandRecieved event's args:
    /// The ID of the command.
    /// The args.
    /// The request's directory's path.
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        public int CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory
        /// <summary>
        /// the Constructor, setting the values.
        /// </summary>
        /// <param name="id">The id of the command we recieved</param>
        /// <param name="args">The arguments to the command </param>
        /// <param name="path">A path of a file/folder</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
