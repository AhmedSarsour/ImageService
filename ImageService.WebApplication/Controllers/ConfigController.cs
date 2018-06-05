using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;
namespace ImageService.WebApplication.Controllers
{
    public class ConfigController : Controller
    {
        ConfigModel model = new ConfigModel();
        // GET: Config

        [HttpGet]
        public ActionResult Config()
        {
            return View(model);
        }


        [HttpGet]
        public ActionResult RemoveHandler(string handler ="")
        {
            return View("RemoveHandler", new RemoveHandlerModel(handler));
        }

        //Checks if the current name exist in the handler (for case that the user put wrong parameters)
        [HttpGet]
        public bool IsExists(string name)
        {
            return model.listHandlers.Contains(name);
        }

        [HttpPost]
        public void Remove(string handler)
        {
            //Remove from the model

            model.RemoveHandler(handler);

        }

        //We want to check it because when we will get ok from service it will be true
        [HttpGet]
        public bool CheckIfRemoved(string handler)
        {
            return model.CheckIfRemoved(handler);
        }
    }
}