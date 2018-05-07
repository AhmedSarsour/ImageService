using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Interfaces;
using ImageService.Infrastructure.Classes;

namespace ImageService.Commands
{
    /// <summary>
    /// The command of close handler of a directory.
    /// </summary>
    public class CloseCommand : ICommand
    {
        private Dictionary<string, IDirectoryHandler> handlers;
        /// <summary>
        /// The closeCommand constructor.
        /// </summary>
        /// <param name="handler">The handler we want to close.</param>
        public CloseCommand(ref Dictionary<string, IDirectoryHandler> handlers)
        {
            //Storing the handler
            this.handlers = handlers;
        }
        /// <summary>
        /// we get the path of the directory, and call the onClose function by the handler.
        /// </summary>
        /// <param name="args">Arguments to the command.</param>
        /// <param name="result">Set to true if succeed and false otherwise</param>
        /// <returns>Message to the logger</returns>
        public string Execute(string[] args, out bool result)
        {
            //Our convention - the path will be stored on args[0]
            //The file path will be stored in args[0] so we will run the add file.
            string path = args[0];

            if (!handlers.ContainsKey(path))
            {
                result = false;
                return "You tried to close a folder that does not exit";
            }

            Configure config = Configure.GetInstance();
            //Remove the handler from the list 
            config.Handlers.Remove(path);
            string handStr = "";
            foreach (string handler in config.Handlers)
            {
                handStr += handler  + ";";
            }
            handStr = handStr.Substring(0, handStr.Length - 1);
            Console.WriteLine(handStr);

            config.UpdateConfig("Handler", handStr);
      
            result = true;
            //Closing the directory we just chose
            return handlers[path].onClose(path);
        }
    }
}
