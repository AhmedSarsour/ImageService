using ImageService.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
namespace ImageService.Commands
{
    /// <summary>
    /// The command that we want to do when a file added to the directory we handle.
    /// </summary>
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;
        /// <summary>
        /// the NewFileCommand constructor.we get the model.
        /// </summary>
        /// <param name="modal">An image modal</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
        /// <summary>
        /// executing the addFile Commmand with the help of the ImageService Model.
        /// </summary>
        /// <param name="args">Arguments to the command.</param>
        /// <param name="result">Set to true if succeed and false otherwise</param>
        /// <returns>Message in this case it will be message to the logger</returns>
        public string Execute(string[] args, out bool result)
        {
            //The file path will be stored in args[0] so we will run the add file.
            return m_modal.AddFile(args[0], out result);
        }
    }
}
