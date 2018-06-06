using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageService.WebApplication.Models
{
    public class RemoveHandlerModel
    {
        [Required]
        [Display(Name = "Handler name")]
        public string HandlerName { get; set; }

        public bool IsConnected()
        {
            ImageService.Communication.Model.IModelCommunication comm = ImageService.Communication.Model.ModelCommunication.GetInstance();

            return comm.IsConnected();
        }

        public RemoveHandlerModel()
        {

        }

        public RemoveHandlerModel(string name)
        {
            this.HandlerName = name;
        }
    }
}