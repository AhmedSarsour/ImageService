using ImageService.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ImageService.Communication
{
    public class AndroidHandler : IClientHandler
    {
        public event Excecute HandlerExcecute;
        public AndroidHandler()
        {

        }
        public void HandleClient(TcpClient c, object locker)
        {
            throw new NotImplementedException();
        }
    }
}
