using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;
namespace ImageService.WebApplication.Controllers
{
    public class LogsController : Controller
    {
        static LogsModel model = new LogsModel();

        // GET: Logs/Logs
        public ActionResult Logs()
        {
            return View(model);
        }

        // POST: First/Logs
        [HttpPost]
        public ActionResult Logs(LogsModel emp)
        {
           return View();
        }
    }
}