using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageService.WebApplication.Models
{
    public class RemoveHandlerModel:TCPModel
    {
        [Required]
        [Display(Name = "Handler name")]
        public string HandlerName { get; set; }

        public RemoveHandlerModel():base()
        {

        }

        public RemoveHandlerModel(string name):base()
        {
            this.HandlerName = name;
        }
    }
}