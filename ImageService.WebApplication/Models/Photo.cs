using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ImageService.WebApplication.Models
{   
    public class Photo
    {
        public string DateShow { get; set; }
        public Photo(string path)
        {}
        public Photo() { }
        public Photo(string path, string name, string date, string thumbPath)
        {
            Path = path;
            DateShow = date;
            Thumbnail = thumbPath;
            string[] picYearMonth = date.Split('/');
            Date = "Year: " + picYearMonth[0] + ", Month: " + picYearMonth[1];
            Name = name;
        }
        public Photo(Photo photo)
        {
            Path = photo.Path;
            Date = photo.Date;
            Name = photo.Name;
            Thumbnail = photo.Thumbnail;
            DateShow = photo.DateShow;
        }
        public void setThumbnail(string thumbPath)
        {
            Thumbnail = thumbPath;
        }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photo Path")]
        public string Path { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photo Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photo's Date")]
        public string Date { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Thumbnail")]
        public string Thumbnail { get; set; }
    }
}