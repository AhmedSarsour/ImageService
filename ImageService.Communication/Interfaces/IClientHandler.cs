using ImageService.Communication.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Communication;
using ImageService.Infrastructure.Interfaces;
namespace ImageService.Communication.Interfaces
{
    public delegate string Excecute (int commandID, string[] args, out bool resultSuccesful);
    public interface IClientHandler
    {
        //The event who in charge of excecute the command
        event Excecute HandlerExcecute;
        void HandleClient(TcpClient c);
    }
}
