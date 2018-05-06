using ImageService.Infrastructure.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Configure configs = new Configure("App.config");
            //Passing the source name and the log name via the parameters in the app config file.
            string[] args = { configs.SourceName, configs.LogName};
            ImageService service = new ImageService(args);
            if (Environment.UserInteractive)
            {
                service.RunAsConsole(args);
            }
            else
            {

                ServiceBase[] ServicesToRun = new ServiceBase[] { new ImageService(args) };
                ServiceBase.Run(ServicesToRun);
                ServicesToRun = new ServiceBase[]
                {
                service
                };
                //Running the service
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
