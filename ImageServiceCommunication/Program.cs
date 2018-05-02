using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication.Interfaces;
using ImageService.Infrastructure.Enums;
using ImageServiceCommunication.Classes;
namespace ImageServiceCommunication
{
    class Program
    {
        static void Main(string[] args)
        {
            Config c = new Config();
            List<string> handlers = new List<string>();
            handlers.Add("handler1");

            c.Handlers = handlers;
            c.LogName = "logname";
            c.OutputDir = "output";
            c.SourceName = "source";
            c.ThumbnailSize = 120;
             

                TcpServer server = new TcpServer(8001, new ClientHandler());
            server.AddJsonAble((int)JsonEnum.SettingsJson,c);

                server.Start();
                server.Stop();
          

        }
    }
}
