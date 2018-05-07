using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Infrastructure.Classes;
using System.Text.RegularExpressions;
using ImageService.Commands;
using ImageService.Communication.Event;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// This class handles the directory we choose - we will use FileSystemWatcher to handle a folder.
    /// </summary>
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher[] m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        #endregion
        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;    
        /// <summary>
        /// the DirectoryHandler constructor.
        /// </summary>
        /// <param name="controller">An image controller</param>
        /// <param name="logger">A logging service</param>
        public DirectoyHandler(IImageController controller, ILoggingService logger)//, string path)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            //Creating array for watchers.
            // Each watcher is for different file type .jpg,.png,.gif,.bmp
            this.m_dirWatcher = new FileSystemWatcher[4];
        }
        /// <summary>
        /// StartHandleDriectory function, it gets a directory path
        /// and handles that directory files.
        /// </summary>
        /// <param name="dirPath">The path of the directory we want to handle</param>
        public void StartHandleDirectory(string dirPath)
        {
                this.m_path = dirPath;
            ///Creating the watchers for each file type;
            string[] fileTypes = { "*.jpg", "*.png", "*.gif", "*.bmp" };
            for (int i = 0; i < fileTypes.Length; i++)
            {
                //Initializing the file system watcher with our dirPath
                m_dirWatcher[i] = new FileSystemWatcher(dirPath);
                m_dirWatcher[i].Filter = fileTypes[i];
                
                //Adding function to occur to our event.
                //This what happan when we add new file
                m_dirWatcher[i].Created += DirectoyHandler_Created;
                //If there is a problem add
                m_dirWatcher[i].EnableRaisingEvents = true;
            }
        }
        /// <summary>
        /// DirectoyHandler_Created function, we get eventArgs e, then we call the OnCommandRecieved function.
        /// </summary>
        /// <param name="sender">Who activated this function</param>
        /// <param name="e">Arguments which helps us to knos what is the folder path</param>
        private void DirectoyHandler_Created(object sender, FileSystemEventArgs e)
        {
            //The argument will be the path of the picture.
            string[] args = { e.FullPath };
            //When someone adds file to our folder we will apply the add file command - i have oncommand recieved so i'll use it
            CommandRecievedEventArgs commandEventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, args[0]);
            //We don't want to write code twice so we will use this function.
            OnCommandRecieved(this, commandEventArgs);
        }

        /// <summary>
        /// OnCommandRecieved function, once we recieved the command we will execute it.
        /// </summary>
        /// <param name="sender">Who sent the command</param>
        /// <param name="e">Arguments of command - id, args</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool resultSuccesful;
            string msg;
            //First we will excecute the command
             msg = m_controller.ExecuteCommand(e.CommandID, e.Args, out resultSuccesful);
            //Than we will write into the logger - we will use our boolean in order to know if succeeded or not
            if (resultSuccesful)
            {
                m_logging.Log(msg, MessageTypeEnum.INFO);
            }
            //Did not succeed
            else
            {
                m_logging.Log(msg, MessageTypeEnum.FAIL);
            }
        }
        /// <summary>
        /// In this part of the excercise we will do it when closing the service.
        /// On advanced part it will be called by invoking the event of dir close and we will stop handling the current folder.
        /// </summary>
        /// <param name="path">The path of the directory we want to stop handle</param>
        /// <returns>Message to logger</returns>
        public string onClose(string path)
        {
            for (int i = 0; i < this.m_dirWatcher.Length; i++)
            {
                //When closing we want the dir watcher will stop to watch our directory.
                // So we will remove the function from the delegate for each watcher.
                m_dirWatcher[i].Changed -= DirectoyHandler_Created;
                //Releasing the dir watcher
                m_dirWatcher[i].EnableRaisingEvents = false;
                m_dirWatcher[i].Dispose();
            }
          
            //Invoking and apply the function we added on image server - OnCloseServer
            DirectoryCloseEventArgs dclose = new DirectoryCloseEventArgs(path, "Directory close");
            DirectoryClose?.Invoke(this, dclose);
            return "The handler of the folder " + this.m_path + " just closed";
        }
    }
}
