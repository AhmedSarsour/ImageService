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

        private void CloseHandler(string path, bool delete)
        {
            Configure config = Configure.GetInstance();
            //Remove the handler from the list 
            config.Handlers.Remove(path);
            //If we want to delete from app config
            if (delete)
            {
                string handStr = "";
                foreach (string handler in config.Handlers)
                {
                    handStr += handler + ";";
                }

                if (config.Handlers.Count == 0)
                {
                    //Give ; because we split by this character
                    handStr = ";";
                }
                handStr = handStr.Substring(0, handStr.Length - 1);
                Console.WriteLine("string is " + handStr);

                config.UpdateConfig("Handler", handStr);

            }
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
            //If we want to delete from app config
            bool delete = false;
            if (args.Length > 1)
            {
                Boolean.TryParse(args[1], out delete);
            }

            //foreach (string key in handlers.Keys)
            //{
            //    Console.WriteLine("Key: " + key);
            //    Console.WriteLine("Value: " + handlers[key]);
            //}

            //Closing all handlers
            Console.WriteLine("Delete is " + delete);

                if (!handlers.ContainsKey(path))
                {
                    result = false;
                    return "You tried to close a folder that does not exit";
                }

                CloseHandler(path, delete);
        
            result = true;
            //Closing the directory we just chose
            return handlers[path].onClose(path);
        }
    }
}
