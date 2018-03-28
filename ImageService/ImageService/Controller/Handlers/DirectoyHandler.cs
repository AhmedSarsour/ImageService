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
        
        public DirectoyHandler(IImageController controller, ILoggingService logger)
        {
            this.m_controller = controller;
            this.m_logging = logger;
            //Creating array for watchers.
            // Each watcher is for different file type .jpg,.png,.gif,.bmp
            this.m_dirWatcher = new FileSystemWatcher[4];

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
                m_dirWatcher[i].Changed += DirectoyHandler_Changed;
              


            }



           
        }

        private void DirectoyHandler_Changed(object sender, FileSystemEventArgs e)
        {
            //The result of the command.
            bool result;
            //The argument willl be the path of the picture.
            string[] args = { e.FullPath };
            //When someone adds file to our folder we will apply the add file command.
            m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, args , out result);
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            throw new NotImplementedException();

            
        }

        // Implement Here!
    }
}
