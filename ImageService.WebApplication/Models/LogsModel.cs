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
    public class LogsModel:TCPModel
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


        private void GetLogs(object sender, string message)
        {
            Logs.FromJson(message);
            gotLogs = true;
        }

        //We want to save the log model as static member at log controller so we need to update the list when log is added.
        private void AddLog(object sender, string message)
        {
            Log log = new Log(1, "");

            log.FromJson(message);
            Logs.AddLog(log);
        }

        public LogsModel():base()
        {
            //Communicate with the server

            Logs = new LogCollection();

            if (!communicate.IsConnected())
            {
                return;
            }
            try
            {
                //Request from the service the logs
                communicate.SendCommend((int)CommandEnum.LogCommand, null);
            }
            catch (Exception)
            {
                return;
            }

            //Updating the communication events
            communicate.GetLogs += GetLogs;
            communicate.AddLog += AddLog;
            //Waiting until adding the logs file until we continue.
            while (!gotLogs) ;
        }
    }
}