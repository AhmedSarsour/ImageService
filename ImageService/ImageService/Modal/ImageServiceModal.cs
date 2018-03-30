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
            string picPath = path;
            string picName = System.IO.Path.GetFileName(picPath);
            // getting  the creation time fo the picture, year and month.
            DateTime time = File.GetCreationTime(picPath);
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
            catch (Exception e)
            {
                result = false;
                return "Problem creating month folder for the image";
            }
            this.m_thumbnailSize = 120;//DONT FOR GETTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT
            Image image = Image.FromFile(monthPath + @"\" + picName);
            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
            thumb.Save(System.IO.Path.ChangeExtension(thumbMonthPath + @"\" + picName, "thumb"));
            try
            {
                File.Delete(picPath);
            }
            catch (Exception e)
            {
                result = false;
                return "Problem deleting the copied picture";
            }
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
