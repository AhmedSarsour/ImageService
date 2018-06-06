using ImageService.Communication.Model;
using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageService.WebApplication.Models
{
    public class LogsModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public LogCollection Logs { get; private set; }

        //Parameters just for display name
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string Type { get; private set; }

        [DataType(DataType.Text)]
        [Display(Name = "Message")]
        public string Message { get; private set; }

        private bool gotLogs = false;
        private ModelCommunication communicate;

        public bool Connected { get; private set; } = false;

        private void GetLogs(object sender, string message)
        {
            Logs.FromJson(message);
            gotLogs = true;
        }

        public LogsModel()
        {
            //Communicate with the server
            communicate = ModelCommunication.GetInstance();

            Logs = new LogCollection();

            if (!communicate.IsConnected())
            {
                Connected = false;
                return;
            }
            try
            {
                //Request from the service the logs
                communicate.SendCommend((int)CommandEnum.LogCommand, null);
                Connected = true;
            }
            catch (Exception)
            {
                Connected = false;
                return;
            }

            //Updating the communication events
            communicate.GetLogs += GetLogs;
            //Waiting until adding the logs file until we continue.
            while (!gotLogs) ;
        }
    }
}