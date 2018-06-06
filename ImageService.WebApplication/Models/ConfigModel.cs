using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageService.Infrastructure.Classes;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ImageService.WebApplication.Models
{
    public class ConfigModel:TCPModel
    {
        private Configure Config { get; set; }
        private bool addedConfig;
        //Dictionary where key is the handler name and the value is if we removed it.

        private Dictionary<string,bool> removes;
        [Required]
        [Display(Name = "Handlers")]
        public List<String> listHandlers { get; set; }

        private void GetConfig(object sender, string message)
        {
            Config.FromJson(message);
            listHandlers = new List<String>(Config.Handlers);
            addedConfig = true;
        }



        private void GetRemoved(object sender, string message)
        {
            //We know that the message from close command will be the handler of the folder x just closed.
            //We want to get x.
            string first = "The handler of the folder ";
            int pFrom = message.IndexOf(first) + first.Length;
            int pTo = message.LastIndexOf(" just closed");
            string folderToClose = "";
            //Found the folders
            if (pFrom != -1 && pTo != -1)
            {
                folderToClose = message.Substring(pFrom, pTo - pFrom);
            }
            //Adding the folder we just removed to the dictionary
            removes[folderToClose] =  true;
        }

        public bool RemoveHandler(string handler)
        {
            ////We didn't remove it yet
            removes.Add(handler, false);

            //We can do it only if connected to the server
            if (communicate.IsConnected())
            {
                try
                {
                     communicate.SendCommend((int)CommandEnum.CloseCommand, new string[] { handler, "true" });
                    //Remove from the model
                    listHandlers.Remove(handler);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;

        }

        public bool CheckIfRemoved(string handler)
        {
            if (!removes.ContainsKey(handler))
            {
                return false;
            }
            //If contains we will return the boolean
            return removes[handler];
        }

        public ConfigModel():base()
        {
            this.removes = new Dictionary<string, bool>();

            //in case the connection had failed.
            if (!communicate.IsConnected())
            {
                return;
            }
            Config = Configure.GetInstance();
            //Request from the service the configurations
            try
            {
                communicate.SendCommend((int)CommandEnum.GetConfigCommand, null);
                communicate.GetConfig += GetConfig;
                communicate.RemoveHandler += GetRemoved;
                //Waiting until adding the config file until we continue;
                while (!addedConfig) ;

                OutPutDir = Config.OutPutDir;
                SourceName = Config.SourceName;
                ThumbnailSize = Config.ThumbnailSize;
                LogName = Config.LogName;
            }
            catch (Exception)
            {
                return;
            }
        }
        [Required]
        [Display(Name = "Output Directory")]
        public string OutPutDir { get; set; }
        //The source name in the event log
        [Required]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }
        //The log name
        [Required]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }
        //The thumbnail picture size
        [Required]
        [Display(Name = "Thumbnail Size")]
        public int ThumbnailSize { get; set; }



    }
}