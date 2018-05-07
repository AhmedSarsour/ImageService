using ImageService.Commands;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Infrastructure.Classes;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication.Event;
using ImageService.Communication;
namespace ImageService.Server
{
    /// <summary>
    /// The server of our project - for now in charge of sending command and create handlers.
    /// </summary>
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved - in more advanced part of this project
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        #endregion
        /// <summary>
        /// IamgeServer constructor, setting the controller and the logger.
        /// </summary>
        /// <param name="controller">The image controller</param>
        /// <param name="logger">Our loogging service</param>
        public ImageServer(IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            //The dictionary of the commands right now has only close server
        }
        /// <summary>
        /// creating the handler on the given directory and beginning to handling it.
        /// </summary>
        /// <param name="pathDirectory">The path of directory we want to create handler for</param>
        public IDirectoryHandler createHandler(string pathDirectory)
        {
            //We are trying to handle directory that does not exist.
            if (!System.IO.Directory.Exists(pathDirectory))
            {
                //Writing to the logger.
                m_logging.Log("The directory in the path " + pathDirectory + " does not exist", MessageTypeEnum.WARNING);
                return null;
            }
            IDirectoryHandler h = new DirectoyHandler(this.m_controller, this.m_logging);
            //By doing this after each creation it will be very easy to close each handler
            CommandRecieved += h.OnCommandRecieved;
            //Adding to the event of the close the closing directory
            h.DirectoryClose += OnCloseServer;
            //Starting to handler the current directory.
            h.StartHandleDirectory(pathDirectory);

            return h;
            
            
        }
        /// <summary>
        /// Invoking the subscribers once the command was Recieved.
        /// </summary>
        public void sendCommand()
        {
            //We dont want to remove the handlers from the app config so we just close them
            string[] args = { "*","false" };
            CommandRecievedEventArgs commandArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, args,"");
            //The invoke will run all the handlers on close server
            CommandRecieved?.Invoke(this, commandArgs);
        }

        //The handler should invoke about this
        /// <summary>
        /// OnCloseServer, we remove the subscribers from the event.
        /// </summary>
        /// <param name="sender">Who called that event (we know its an handler)</param>
        /// <param name="e">Arguments for the event</param>
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
