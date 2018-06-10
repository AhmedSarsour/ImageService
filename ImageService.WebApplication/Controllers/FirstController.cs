using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;
using System.IO;
using System.Web.Hosting;

namespace ImageService.WebApplication.Controllers
{
    public class FirstController : Controller
    {
        private static List<Student> ReadStudentsFile()
        {
            List<Student> students = new List<Student>();
          //  string dir = Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory());

            string path = HostingEnvironment.MapPath("~/App_Data/Students.txt");

            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    students.Add(new Student(line));
                }
            } catch(Exception)
            {

            }
            return students;
        }

        static List<Student> students = ReadStudentsFile();

        // GET: First/ImageWeb
        public ActionResult ImageWeb()
        {
            ImageWebModel model = new ImageWebModel();
            model.NumPhotos = new PhotosModel().NumPhotos;
            model.Students = students;
            return View(model);
        }


    

    }
}
