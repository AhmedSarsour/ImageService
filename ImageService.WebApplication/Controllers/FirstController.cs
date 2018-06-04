using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.WebApplication.Models;

namespace ImageService.WebApplication.Controllers
{
    public class FirstController : Controller
    {
        static List<Student> students = new List<Student>()
        {
          new Student  { FirstName = "Moshe", LastName = "Aron",ID = 206 },
          new Student  { FirstName = "Dor", LastName = "Nisim", ID = 207 }
        };
    


        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }


        // GET: First/ImageWeb
        public ActionResult ImageWeb()
        {
            return View(students);
        }

    

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Student emp in students) {
                if (emp.ID.Equals(id)) { 
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Student empT)
        {
            try
            {
                foreach (Student st in students)
                {
                    if (st.ID.Equals(id))
                    {
                        st.copy(empT);
                        return RedirectToAction("Photos");
                    }
                }

                return RedirectToAction("Photos");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Student emp in students)
            {
                if (emp.ID.Equals(id))
                {
                    students.RemoveAt(i);
                    return RedirectToAction("ImageWeb");
                }
                i++;
            }
            return RedirectToAction("Error");
        }
    }
}
