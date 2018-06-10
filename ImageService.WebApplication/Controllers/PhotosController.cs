using ImageService.WebApplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ImageService.WebApplication.Controllers
{
    public class PhotosController : Controller
    {
       

        //Not static in case of photo added
        PhotosModel imgModel = new PhotosModel();
        public IEnumerable<Photo> PathsNumer { get; set; }
        // GET: Photos
        public ActionResult Photos()
        {
            return View(imgModel);
        }

        public ActionResult ViewPhoto(Photo photo)
        {
            return View(photo);
        }
        public ActionResult DeletePhoto(Photo photo)
        {
            return View(photo);
        }
        [HttpPost]
        public string Delete(string photoName)
        {
            return (imgModel.RemovePhoto(photoName));
        }
    }
}
