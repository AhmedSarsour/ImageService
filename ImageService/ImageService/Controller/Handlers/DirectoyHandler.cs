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
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;

namespace ImageService.Controller.Handlers
{
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
        
        public DirectoyHandler(IImageController controller, ILoggingService logger, string path)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            //Creating array for watchers.
            // Each watcher is for different file type .jpg,.png,.gif,.bmp
            this.m_dirWatcher = new FileSystemWatcher[4];

            this.m_path = path;

        }

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
         //       m_dirWatcher[i].Deleted += 
            }
        }

        private void DirectoyHandler_Created(object sender, FileSystemEventArgs e)
        {
            //The result of the command.
            bool result;
            //The argument willl be the path of the picture.
            string[] args = { e.FullPath };
            //When someone adds file to our folder we will apply the add file command.
            m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args , out result);
            //When someone adds file we will write it into the logs file
            m_logging.MessageRecieved += M_logging_MessageRecieved;
        }

        private void M_logging_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            m_logging.Log(e.Message, e.Status);
            
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool b;
            m_controller.ExecuteCommand(e.CommandID, e.Args, out b);   
        }

        public void onCloseServer(object sender, CommandRecievedEventArgs e)
        {
            for (int i = 0; i < this.m_dirWatcher.Length; i++)
            {
                //When closing we want the dir watcher will stop to watch our directory.
                // So we will remove the function from the delegate for each watcher.
                m_dirWatcher[i].Changed -= DirectoyHandler_Created;
            }

            DirectoryCloseEventArgs dclose = new DirectoryCloseEventArgs(e.RequestDirPath, "Dire");

            
            DirectoryClose?.Invoke(this, dclose);




        }

        // Implement Here!
    }
}
