using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageService.WebApplication.Models
{
    public class Student
    {
        public Student()
        {
        }


        public Student(string line)
        {
            string[] subStrings = line.Split(';');
            try
            {
                ID = int.Parse(subStrings[0]);
                FirstName = subStrings[1];
                LastName = subStrings[2];
            } catch(Exception)
            {

            }

            
        }
        public void copy(Student st)
        {
            FirstName = st.FirstName;
            LastName = st.LastName;
        }
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

    }
}