using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui.Model
{
    public interface IModelCommunication
    {
        //We create events which we will invoke by the message we got
        event EventHandler<string> AddLog;
        event EventHandler<string> RemoveHandler;
        event EventHandler<string> GetConfig;
        event EventHandler<string> GetLogs;

        void SendCommend(int id, string[] args);
        bool IsConnected();





    }
}
