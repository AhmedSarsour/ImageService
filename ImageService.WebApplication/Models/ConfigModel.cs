using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageService.Infrastructure.Classes;
using ImageService.Communication.Model;
using ImageService.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace ImageService.WebApplication.Models
{
    public class ConfigModel
    {
        private Configure Config { get; set; }
        private bool addedConfig;
        [Required]
        [Display(Name = "Handlers")]
        public List<String> listHandlers { get; set; }
        public bool Connected { get; private set; }

        private void GetConfig(object sender, string message)
        {
            Config.FromJson(message);
            listHandlers = new List<String>(Config.Handlers);
            addedConfig = true;
        }

        public ConfigModel()
        {
            Connected = false;
            //getting a communcation instance.
            IModelCommunication communicate = ModelCommunication.GetInstance();
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
                //Waiting until adding the config file until we continue;
                while (!addedConfig) ;

                OutPutDir = Config.OutPutDir;
                SourceName = Config.SourceName;
                ThumbnailSize = Config.ThumbnailSize;
                LogName = Config.LogName;
                Connected = true;
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