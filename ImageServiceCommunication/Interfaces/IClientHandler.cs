using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication.Interfaces
{
    public interface IClientHandler
    {
        void HandleClient(TcpClient c);
    }
}
