using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Modal;

namespace ImageService.ImageService.Commands
{
    public class CloseCommand : ICommand
    {
        //We told to have only addFile on imageServiceModal interface so in order to use the close command
        // we cant use the interface so we use the m_modal.
        private ImageServiceModal m_modal;

        public CloseCommand(ImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            //The file path will be stored in args[0] so we will run the add file.
            return m_modal.CloseService(out result);
        }
    }
