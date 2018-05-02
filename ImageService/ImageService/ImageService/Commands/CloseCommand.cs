using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Interfaces;
namespace ImageService.Commands
{
    /// <summary>
    /// The command of close handler of a directory.
    /// </summary>
    public class CloseCommand : ICommand
    {
        private IDirectoryHandler m_handler;
        /// <summary>
        /// The closeCommand constructor.
        /// </summary>
        /// <param name="handler">The handler we want to close.</param>
        public CloseCommand(IDirectoryHandler handler)
        {
            //Storing the handler
            this.m_handler = handler;
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
            string path = args[0];
            result = true;
            //The file path will be stored in args[0] so we will run the add file.
            return m_handler.onClose(path);
        }
    }
}
