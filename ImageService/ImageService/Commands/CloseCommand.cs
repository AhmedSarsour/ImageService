using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;
using ImageService.Controller.Handlers;

namespace ImageService.Commands
{
    public class CloseCommand : ICommand
    {
        private IDirectoryHandler m_handler;

        public CloseCommand(IDirectoryHandler handler)
        {
            //Storing the handler
            this.m_handler = handler;
        }

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
