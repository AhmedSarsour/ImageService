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
    class ImageFolderFunctions
    {
        /// <summary>
        ///Create directory if it it does not exist.
        /// </summary>
        /// <param name="path"></param>
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
        //it isn't stressing the garbage man
        private static Regex r = new Regex(":");

        /// <summary>
        /// retrieves the datetime WITHOUT loading the whole image.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
