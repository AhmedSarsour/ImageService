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
        private static List<Photo> ReadImagesFile()
        {
            List<Photo> imagePaths = new List<Photo>();
            try
            {
                //getting the years directories
                string[] dirs = Directory.GetDirectories(HostingEnvironment.MapPath("~/OutputFolder"));
                //running on all the years directories.
                foreach (string dir in dirs)
                {
                    //getting the directory name.
                    string yearDir = new DirectoryInfo(dir).Name;
                    //we dont enter the thumbnails directory.
                    if (!yearDir.Equals("thumbnails"))
                    {
                        //getting the months directories
                        try
                        {
                            string[] months = Directory.GetDirectories(dir);
                            //running on the months directories
                            foreach (string monthDir in months)
                            {
                                //getting the pictures.
                                try
                                {
                                    string[] files = Directory.GetFiles(monthDir); //Getting all files
                                    //adding the pictures to the list.
                                    foreach (string file in files)
                                    {
                                        string imgPath = "OutputFolder/" + yearDir + @"/" + new DirectoryInfo(monthDir).Name + @"/" + Path.GetFileName(file);
                                        string thumbPath = "OutputFolder/thumbnail/" + yearDir + @"/" + new DirectoryInfo(monthDir).Name
                                           + @"/" + Path.GetFileName(file);
                                        //System.IO.File.Move(HostingEnvironment.MapPath("~/" + thumbPath),
                                        //      Path.ChangeExtension(HostingEnvironment.MapPath("~/" + thumbPath), "jpg" /*Path.GetExtension(file)*/));
                                        try
                                        {
                                            string date = yearDir + "/" + new DirectoryInfo(monthDir).Name;
                                            imagePaths.Add(new Photo(imgPath, Path.GetFileName(file), date, thumbPath));
                                        }
                                        catch (Exception)
                                        {
                                            imagePaths.Add(new Photo("failed adding to img"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    imagePaths.Add(new Photo("Failed in getting the Pictures!"));
                                    imagePaths.Add(new Photo(monthDir));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            imagePaths.Add(new Photo("Failed in opening months directories"));
                            imagePaths.Add(new Photo(dir));
                        }
                    }
                }
            }
            catch (Exception)
            {
                imagePaths.Add(new Photo("Failed in opening Years Dirs"));
            }
            return imagePaths;
        }

        //Not static in case of photo added
        PhotosModel imgModel = new PhotosModel(ReadImagesFile());
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
