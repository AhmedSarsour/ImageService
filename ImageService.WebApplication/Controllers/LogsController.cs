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
        // GET: First/Logs
        public ActionResult Logs()
        {
            return View();
        }

        // POST: First/Logs
        [HttpPost]
        public ActionResult Logs(Employee emp)
        {
            try
            {
                //employees.Add(emp);

                return RedirectToAction("ImageWeb");
            }
            catch
            {
                return View();
            }
        }
    }
}