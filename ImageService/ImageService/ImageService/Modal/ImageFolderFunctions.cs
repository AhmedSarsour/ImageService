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
        /// Change picture path to new path with name we want.
        /// </summary>
        /// <param name="path">The path of the picture</param>
        /// <param name="newName">The new name we want to the picture</param> 
        /// <returns>The new path of the picture</returns>
        public static string changeNameImagePath(string path, string newName )
        {
            //Getting the name of the picture
            string file = Path.GetFileNameWithoutExtension(path);
            string NewPath = path.Replace(file, newName);
            return NewPath;
        }
        /// <summary>
        /// Adding string name of a picture for example pic.jpg to pic(1).jpg
        /// </summary>
        /// <param name="path">The path of the picture</param>
        /// <param name="newName">The new name we want to the picture</param> 
        /// <returns>The new path of the picture</returns>
        public static string addToImagePath(string path, string extention)
        {
            string file = Path.GetFileNameWithoutExtension(path);
            //Adding the extention to the file name
            return changeNameImagePath(path, file + extention);
        }
        /// <summary>
        /// Making folder to hidden folder
        /// </summary>
        /// <param name="folder">The folder we want to make  hidden</param>
        public static void MakeFolderHidden(string folder)
        {
            //getting the information on the folder
            DirectoryInfo di = new DirectoryInfo(folder);
            if (di != null)
            {
                //if the file isn't hidden then we make it hidden.
                if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    di.Attributes |= FileAttributes.Hidden;
                }
            }
        }

  



    }
}
