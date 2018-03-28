//using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        #endregion

        public string AddFile(string path, out bool result)
        {
            if (!File.Exists(path))
            {
                result = false;
                return "the image you gave doesn't Exist!.";
            }

            //checking of the outputFolder exists or not.
            if (!Directory.Exists(m_OutputFolder))
            {
                //in case it doesn't exist, we create it.
                m_OutputFolder = @"c:\OutputFolder";
                System.IO.Directory.CreateDirectory(m_OutputFolder);
            }
            //getting the path and name of the picture.
            //string picPath = System.IO.Path.GetPathRoot(path);
            string picPath = path;

            string picName = System.IO.Path.GetFileName(picPath);
            // getting  the creation time fo the picture, year and month.
            DateTime time = File.GetCreationTime(picPath);
            int picYear = time.Year;
            int picMonth = time.Month;
            //getting the path to year directory in the outputFolder directory
            string yearPath = System.IO.Path.Combine(m_OutputFolder, picYear.ToString());
            //if the year directory doesn't exist, we create it.
            if (!Directory.Exists(yearPath))
            {
                System.IO.Directory.CreateDirectory(yearPath);
            }
            //getting te path to month directory in the year directory.
            string monthPath = System.IO.Path.Combine(yearPath, picMonth.ToString());
            //if the month directory doesn't exist, we create it.
            if (!Directory.Exists(monthPath))
            {
                System.IO.Directory.CreateDirectory(monthPath);
            }
            //moving the image into outputFile.
            System.IO.File.Move(picPath, monthPath + @"\" + picName);
            //setting result to true since the image moved successfully.
            result = true;
            return "Image:" + picName + "added successfully: " + monthPath;
        }


    }
}
