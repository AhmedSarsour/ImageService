using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    ///Holds functions for image and folders
    ///</summary>
    class ImageFolderFunctions
    {
        /// <summary>
        ///Create directory if it it does not exist.
        ///</summary>
        ///<param name="path">The path of the directory we want to create</param>
        public static void CreateDirectory(string path)  
        {
            //checking if the directory doesn't exist.
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                } 
                catch
                {
                    throw new Exception("Problem creating the folder in the path: " + path);
                }
            }
        }

        //we init this once so that if the function is repeatedly called
        private static Regex r = new Regex(":");

        /// <summary>
        /// Retrieves the datetime WITHOUT loading the whole image. - this function I took from the internet.
        /// </summary>
        /// <param name="path">The path of the picture</param>
        /// <returns>The date the image was taken</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}
