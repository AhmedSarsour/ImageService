using ImageService.Communication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageService.WebApplication.Models
{
    public class TCPModel
    {
        protected IModelCommunication communicate = null;

        public bool IsConnected()
        {
            return communicate.IsConnected();
        }

        public TCPModel()
        {
            //getting a communcation instance.
            communicate = ModelCommunication.GetInstance();
        }



    }
}