using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Model
{
    /// <summary>
    /// interface for the communication.
    /// </summary>
    public interface IModelCommunication
    {
        //We create events which we will invoke by the message we got
        event EventHandler<string> AddLog;
        event EventHandler<string> RemoveHandler;
        event EventHandler<string> GetConfig;
        event EventHandler<string> GetLogs;
        //sends the command to the client.
        void SendCommend(int id, string[] args);
        //checking whether the connection(between client and server) was true or false.
        bool IsConnected();
    }
}
