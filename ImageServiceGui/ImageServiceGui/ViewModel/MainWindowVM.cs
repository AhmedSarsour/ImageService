using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGui.Model;
namespace ImageServiceGui
{
    /// <summary>
    /// the main window's view Model. connects between the mainwindow's view and model.
    /// </summary>
    class MainWindowVM
    {
        public string Color { get; set; }
        private MainWindowModel mwModel;
        /// <summary>
        /// the mainwindowVM constructor, defining things based on the connection to the server.
        /// </summary>
        public MainWindowVM()
        {
            this.mwModel = new MainWindowModel();
            if (this.mwModel.IsConnected())
            {
                Color = "White";
            }
            else
            {
                Color = "Gray";
            }
        }
    }
}
