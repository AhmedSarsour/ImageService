using ImageServiceCommunication.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageServiceCommunication;
namespace ImageServiceCommunication.Interfaces
{
    public delegate Jsonable JsonableEvent(object sender, JsonSendEventArgs args);
    public interface IClientHandler
    {
        event JsonableEvent JSEvent;
        void HandleClient(TcpClient c);
    }
}
