using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageService.Communication.Model;
using System.ComponentModel.DataAnnotations;
using System.Web.Hosting;

namespace ImageService.WebApplication.Models
{
    public class PhotosModel
    {
        private IEnumerable<Photo> photos;
        public IEnumerable<Photo> Photos { get { return photos; } set { photos = value; } }
        public Photo PhotoInfo { get; }
        public PhotosModel(List<Photo> list)
        {
            photos = list;
        }
        public string RemovePhoto(string photoName)
        {
            List<Photo> list = new List<Photo>(Photos);
            try
            {
                Photo img = FindPhotoByName(photoName);
                string path1 = img.Thumbnail;
                string path2 = img.Path;
                string file1 = HostingEnvironment.MapPath("~/" + img.Thumbnail);
                string file2 = HostingEnvironment.MapPath("~/" + img.Path);

                if (System.IO.File.Exists(file1))
                {
                    int sleeps = 0;
                    //We give a lot of time of 25 seconds

                    while (sleeps < 50) {
                        try
                        {
                            System.IO.File.Delete(file1);
                                break;
                        }
                        catch(Exception)
                        { 
                            // A little sleep between each iteration
                            System.Threading.Thread.Sleep(500);
                            sleeps++;
                        }
                    }

                    if (sleeps > 50)
                    {
                        throw new Exception("Problem deleting the file of thumbnail");
                    }
                }
                if (System.IO.File.Exists(file2))
                {
                    int sleeps = 0;
                    //We give a lot of time of 25 seconds
                    while (sleeps < 50)
                    {
                        try
                        {
                            System.IO.File.Delete(file2);
                            break;
                        }
                        catch (Exception)
                        {
                            // A little sleep between each iteration
                            System.Threading.Thread.Sleep(500);
                            sleeps++;
                        }
                    }

                    if (sleeps > 50)
                    {
                        throw new Exception("Problem deleting the file of image");
                    }
                }
                list.Remove(FindPhotoByName(photoName));
                Photos = list;
                return photoName;
            }
            catch (Exception e) {
                string exception = e.Message;
                return string.Empty; }
        }
        public Photo FindPhotoByName(string name)
        {
            List<Photo> list = new List<Photo>(Photos);
            foreach(Photo img in list)
            {
                if (img.Name.Equals(name))
                {
                    return img;
                }
            }
            //no such photo.
            return null;
            
        }
    }
}