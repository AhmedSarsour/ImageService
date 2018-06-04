using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageService.WebApplication.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Config

        [HttpGet]
        public ActionResult Config()
        {
            return View();
        }
    }
}