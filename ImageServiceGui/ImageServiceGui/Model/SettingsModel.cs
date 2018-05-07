using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
namespace ImageServiceGui.Model
{
    class SettingsModel :ISettingsModel
    {
        public SettingsModel()
        {
            Configure config = Configure.GetInstance();
        }
   
    }
}
