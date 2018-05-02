using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Classes;
namespace ImageServiceGui.Model
{
    class SettingsModel :ISettingsModel
    {
        public SettingsModel()
        {
            Config config = new Config();
        }
   
    }
}
