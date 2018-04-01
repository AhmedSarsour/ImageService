using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// The arguements of teh DirectoryClose event:
    /// the path of the directory
    /// the message to give to the logger.
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        public string DirectoryPath { get; set; }
        // The Message That goes to the logger
        public string Message { get; set; }             
        /// <summary>
        /// the Constructor, setting the values.
        /// </summary>
        /// <param name="dirPath">The directory path</param>
        /// <param name="message">The message we want to pass </param>
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }

    }
}
