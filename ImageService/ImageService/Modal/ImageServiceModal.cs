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
    /// <summary>
    ///  This class is our image model.
    ///  It is incharge of creating folders for the image and creating a thumbnail.
    /// </summary>
    /// 
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        /// <summary>
        /// the ImageServiceModal constructor, getting the configurations from app.config:
        /// setting the path of the output directory,
        /// and getting the thumbnail size.
        /// </summary>
        public ImageServiceModal()
        {
            //The app config file is on the previous folder
            Configure configs = new Configure(@"..\App.config");
            //Taking the outputFolder and the thumbnail size from the app config.
            this.m_OutputFolder = configs.OutPutDir;
            this.m_thumbnailSize = configs.ThumbnailSize;
        }
        /// <summary>
        /// AddFile function, getting a path to a file, adding it to the output directory.
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="result">An out variable which will be true or false in accordance for fail or success </param>
        /// <returns>The result if file added successfully or not - this is the message we write into the log.</returns>
        public string AddFile(string path, out bool result)
        {
            if (!File.Exists(path))
            {
                result = false;
                return "The image you gave doesn't Exist!.";
            }
            //getting the path and name of the picture.
            string picPath = path;
            string picName = System.IO.Path.GetFileName(picPath);
            // Getting  the Creation time fo the picture, year and month.
            // If i used getcreationtime it wasn't the real creation time - we need the last time someone wrote to it
            //because when someone create picture first it is the first time he wrote to it.
            DateTime time = File.GetLastWriteTime(picPath);
            int picYear = time.Year;
            int picMonth = time.Month;
            //getting the path to year directory in the outputFolder directory
            string yearPath = System.IO.Path.Combine(m_OutputFolder, picYear.ToString());
            //getting te path to month directory in the year directory.
            string monthPath = System.IO.Path.Combine(yearPath, picMonth.ToString());
            //Building the thumbnail's path
            string thumbPath = Path.Combine(m_OutputFolder, "thumbnail");
            //The thumbnail's year path
            string thumbYearPath = Path.Combine(thumbPath, picYear.ToString());
            //The thumbnail's month path
            string thumbMonthPath = Path.Combine(thumbYearPath, picMonth.ToString());
            //If we added the extention
            string newPicName = "";
            //Building the directories
            try
            {
                //checking of the outputFolder exists or not- in case it doesn't exist, we create it. 
                ImageFolderFunctions.CreateDirectory(m_OutputFolder);
                //getting the information on the outputFolder
                DirectoryInfo di = new DirectoryInfo(m_OutputFolder);
                //if the file isn't hidden then we make it hidden.
                if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    di.Attributes |= FileAttributes.Hidden;
                }
                //if the year directory doesn't exist, we create it.
                ImageFolderFunctions.CreateDirectory(yearPath);
                //if the month directory doesn't exist, we create it.
                ImageFolderFunctions.CreateDirectory(monthPath);
                //creating the thumbnail directory
                ImageFolderFunctions.CreateDirectory(thumbPath);
                //creating the year directory in the thumbnail, if not existed, we create it.
                ImageFolderFunctions.CreateDirectory(thumbYearPath);
                //creating the month directory in thumbnail, if not existed, we create it.
                ImageFolderFunctions.CreateDirectory(thumbMonthPath);
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            //Copy the picture after creating all the directories
            try
            {
                newPicName = picName;
                //copying the image into the output folder.
                string newPath = monthPath + @"\" + picName;
                //First checking if it is already in the output folder
                if (!File.Exists(newPath))
                {
                    System.IO.File.Copy(picPath, newPath);
                }
                //The file is exist so we will copy in other name - for example for pic.jpg we will save pic_copy.jpg
                else
                {
                    while (File.Exists(newPath))
                    {
                        //Adding _copy until we have name of picture we do not have.
                        newPath = ImageFolderFunctions.addToImagePath(newPath, "_1");
                    }
                    newPicName = Path.GetFileName(newPath);
                    System.IO.File.Copy(picPath, newPath);
                    //Adding copy to the pic name.
                }
            }
            catch (Exception e)
            {
                result = false;
                return "Problem copying the image into the output folder\n\nImage:" + picPath + "\n\n" + e.ToString();
            }
            Image image = Image.FromFile(monthPath + @"\" + picName);
            //Creating the thumbnail.
            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);
            try
            {

                string newThumbPath = thumbMonthPath + @"\" + newPicName;
                //First check if the thumbnail is already in the output folder
                if (!File.Exists(thumbMonthPath + @"\" + newPicName))
                {
                    thumb.Save(System.IO.Path.ChangeExtension(newThumbPath, "thumb"));
                }
            }
            catch
            {
                result = false;
                return "Problem saving the thumbnail picture\n\nImage:" + picPath;
            }
            //Delete the original picture
            try
            {
                File.Delete(picPath);
            }
            catch
            {
                result = false;
                return "Problem deleting the copied picture";
            }

            //setting result to true since the image moved successfully.
            result = true;
            return "Image: " + newPicName + " was added successfully\n\nIt is on the path: " + monthPath;

        }
    }
}
