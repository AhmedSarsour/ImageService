using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication.Interfaces;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Classes;
namespace ImageService.Communication
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure c = new Configure();
            List<string> handlers = new List<string>();
            handlers.Add("handler1");

            c.Handlers = handlers;
            c.LogName = "logname";
            c.OutPutDir = "output";
            c.SourceName = "source";
            c.ThumbnailSize = 120;
             

                TcpServer server = new TcpServer(8001, new ClientHandler());
            server.AddJsonAble((int)JsonEnum.SettingsJson,c);

                server.Start();
                server.Stop();
          

        }
    }
}
