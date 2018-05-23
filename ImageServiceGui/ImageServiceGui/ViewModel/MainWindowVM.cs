using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGui.Model;
namespace ImageServiceGui
{
    class MainWindowVM
    {
        public string Color { get; set; }
        private MainWindowModel mwModel;
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
