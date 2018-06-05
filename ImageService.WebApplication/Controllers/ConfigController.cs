using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;
using System.Diagnostics;

namespace ImageService.WebApplication.Controllers
{
    public class ConfigController : Controller
    {
        static ConfigModel model = new ConfigModel();
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
        public bool Delete(string name)
        {
            //Remove from the model
            return model.RemoveHandler(name);

        }

        //We want to check it because when we will get ok from service it will be true
        public bool CheckIfRemoved(string handler)
        {
            return model.CheckIfRemoved(handler);
        }
        [HttpPost]
        public bool WaitUntilRemoved(string name)
        {
            bool removed = false;
            // Create new stopwatch.
    //        Stopwatch stopwatch = new Stopwatch();
         //   stopwatch.Start();
            while (!removed)
            {
                removed = CheckIfRemoved(name);
                ////Passed 50 seconds - we want to avoid busy waiting
                //if (stopwatch.ElapsedMilliseconds == 50000)
                //{
                //    break;
                //}
            }
      //      stopwatch.Stop();


            return removed;
        }
    }
}