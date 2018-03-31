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

        public ImageServiceModal()
        {
            //Taking the outputFolder and the thumbnail size from the app config.
            this.m_OutputFolder = @"C:\Users\eliad1998\Documents\אוניברסיטה\תכנות מתקדם 2\תרגילי בית\תרגיל 1\OutputFolder";
            this.m_thumbnailSize = 120;

        }
        public string AddFile(string path, out bool result)
        {
            if (!File.Exists(path))
            {
                result = false;
                return "The image you gave doesn't Exist!.";
            }

            //checking of the outputFolder exists or not- in case it doesn't exist, we create it. 
            ifNotExistCreate(m_OutputFolder);

            //getting the path and name of the picture.
            string picPath = path;
            string picName = System.IO.Path.GetFileName(picPath);
            // Getting  the Creation time fo the picture, year and month.
            // If i used getcreationtime it wasn't the real creation time - i want the last time someone wrote to it because when someone create picture first 
            // it is the first time he wrote to it
            DateTime time = File.GetLastWriteTime(picPath);
            int picYear = time.Year;
            int picMonth = time.Month;
            //getting the path to year directory in the outputFolder directory
            string yearPath = System.IO.Path.Combine(m_OutputFolder, picYear.ToString());
            //if the year directory doesn't exist, we create it.
            this.ifNotExistCreate(yearPath);
            //getting te path to month directory in the year directory.
            string monthPath = System.IO.Path.Combine(yearPath, picMonth.ToString());
            //if the month directory doesn't exist, we create it.
            this.ifNotExistCreate(monthPath);

            //creating the thumbnail directory
            string thumbPath = Path.Combine(m_OutputFolder, "thumbnail");
            this.ifNotExistCreate(thumbPath);

            //creating the year directory in the thumbnail, if not existed, we create it.
            string thumbYearPath = Path.Combine(thumbPath, picYear.ToString());
            this.ifNotExistCreate(thumbYearPath);

            //creating the month directory in thumbnail, if not existed, we create it.
            string thumbMonthPath = Path.Combine(thumbYearPath, picMonth.ToString());
            this.ifNotExistCreate(thumbMonthPath);

            try
            {
                //copying the image into the output folder.
                System.IO.File.Copy(picPath, monthPath + @"\" + picName);
            }
            catch 
            {
                result = false;
                return "Problem copying the image into the output folder\n\nImage:" + picPath;
            }
            Image image = Image.FromFile(monthPath + @"\" + picName);
            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);

            try
            {
                thumb.Save(System.IO.Path.ChangeExtension(thumbMonthPath + @"\" + picName, "thumb"));
            }
            catch
            {
                result = false;
                return "Problem saving the thumbnail picture\n\nImage:" + picPath;
            }
            ////Delete the original picture
            //try
            //{
            //    File.Delete(picPath);
            //}
            //catch
            //{
            //    result = false;
            //    return "Problem deleting the copied picture";
            //}

            //setting result to true since the image moved successfully.
            result = true;
            return "Image:" + picName + " was added successfully: " + monthPath;
        }
        public void ifNotExistCreate(string path)
        {
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
