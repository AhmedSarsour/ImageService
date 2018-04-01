using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// This interface is for class which handles the directory we choose - we will use FileSystemWatcher to handle a folder.
    /// </summary>
    public interface IDirectoryHandler
    {
        // The Event That Notifies that the Directory is being closed.   
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        /// <summary>                                                                   
        /// The Function Recieves the directory to Handle.                                                                       
        /// </summary>                                                                        
        /// <param name="dirPath">The path of the directory we want to start handle</param>                                                             
        void StartHandleDirectory(string dirPath);
        /// <summary>
        /// OnCommandRecieved function, once we recieved the command we will execute it.
        /// </summary>
        /// <param name="sender">Who sent the command</param>
        /// <param name="e">Arguments of command - id, args</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command.

        /// <summary>
        /// A function called when closing the handler.
        /// </summary>
        /// <param name="path">The path of the directory we want to stop handle</param>
        /// <returns>Message</returns>
        string onClose(string path);
    }
}
