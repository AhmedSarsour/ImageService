using ImageService.Commands;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private Dictionary<int, ICommand> commands;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved - in more advanced part of this project
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

   

        #endregion

        public ImageServer(IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            //The dictionary of the commands right now has only close server
        }

        public void createHandler(string pathDirectory)
        {
            IDirectoryHandler h = new DirectoyHandler(this.m_controller, this.m_logging);
            //By doing this after each creation it will be very easy to close each handler
            CommandRecieved += h.OnCommandRecieved;
            //Adding to the event of the close the closing directory
            h.DirectoryClose += OnCloseServer;

            //Starting to handler the current directory.
            h.StartHandleDirectory(pathDirectory);
            
            
        }

        public void sendCommand()
        {
            string[] args = { "*" };
            CommandRecievedEventArgs commandArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args,"");
            //The invoke will run all the handlers on close server
            CommandRecieved?.Invoke(this, commandArgs);
        }

        //The handler should invoke about this
        public void OnCloseServer(object sender, DirectoryCloseEventArgs e)
        {
            //Before we cast sender to IDirectoryHandler we need to check if it's type.
            if (sender is IDirectoryHandler)
            {
                IDirectoryHandler h = (IDirectoryHandler)sender;
                //OnClosing server we will remove those function from event.
                CommandRecieved -= h.OnCommandRecieved;
            }
        }



    }
}
