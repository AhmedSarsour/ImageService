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
    /// <summary>
    /// delegate for executing the commands.
    /// </summary>
    /// <param name="commandID"></param>
    /// <param name="args"></param>
    /// <param name="resultSuccesful"></param>
    /// <returns></returns>
    public delegate string Excecute (int commandID, string[] args, out bool resultSuccesful);
    public interface IClientHandler
    {
        //The event who in charge of excecute the command
        event Excecute HandlerExcecute;
        //a function that will handle all the client's requests.
        void HandleClient(TcpClient c, object locker);
    }
}
