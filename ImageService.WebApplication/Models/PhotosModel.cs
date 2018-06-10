using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImageService.Communication.Model;
using System.ComponentModel.DataAnnotations;
using System.Web.Hosting;
using System.IO;

namespace ImageService.WebApplication.Models
{
    public class PhotosModel
    {
        private IEnumerable<Photo> photos;
        public IEnumerable<Photo> Photos { get { return photos; } set { photos = value; } }
        public Photo PhotoInfo { get; }

        public int NumPhotos { get; private set; }

        //Reading all the images from the directory
        private List<Photo> ReadImagesFile()
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
        public PhotosModel()
        {
            photos = ReadImagesFile();
            NumPhotos = photos.Count();
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