using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private static Regex r = new Regex(":");

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="m_OutputFolder"></param>
        /// <param name="m_thumbnailSize"></param>
        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                if (!Directory.Exists(m_OutputFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(m_OutputFolder);
                    di.Attributes = FileAttributes.Hidden;
                }
                // get date and time
                Thread.Sleep(300); //wait for image being prefectly in directory.
                DateTime imageDate = GetDateTakenFromImage(path);
                // call for moving normally the image
                string saveNewImagePath = setInDir(imageDate, path, false, null);
                // call for creating thumbnail image
                setInDir(imageDate, path, true, saveNewImagePath);
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            result = true;
            return "Adding File completed successfully at the path: " + path;
        }

        /// <summary>
        /// get the date of picture being taken.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                if (myImage.PropertyIdList.Any(p => p == 36867))
                {
                    // if there is the date in GetPropertyItem(36867), save it
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                } // if no info exists take the creation time
                return File.GetCreationTime(path);
            }
        }

        /// <summary>
        /// save the thumbnail.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="thumnmailDirPath"></param>
        public void handleThumbnailSize(string path, string thumnmailDirPath)
        {
            using (Image thumb = Image.FromFile(path))
            using (Image newIm = thumb.GetThumbnailImage(
              m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero))
            {
                newIm.Save(thumnmailDirPath);
                File.SetCreationTime(thumnmailDirPath, GetDateTakenFromImage(path));
            }
        }

        /// <summary>
        /// get the name of the image if there is an image 
        /// with the same name its add ({num}).
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns>the name of image</returns>
        public string getName(string path, string file)
        {
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(file);
            string extension = Path.GetExtension(file);
            string newFullPath = Path.Combine(path, Path.GetFileName(file));
            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }

        /// <summary>
        /// this function set the image in the dirctory.
        /// if the dirctory doesn't exist it cretae it.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="path"></param>
        /// <param name="thumbnail">bool to check if thubnail creation or regular.</param>
        /// <param name="saveNewImagePath"></param>
        /// <returns></returns>
        public string setInDir(DateTime date, string path, bool thumbnail, string saveNewImagePath)
        {
            string targetDir = m_OutputFolder;
            // normal copy
            if (thumbnail)
            {
                targetDir = Path.Combine(m_OutputFolder, "Thumbnails");
            }
            // get info
            int year = date.Year;
            int month = date.Month;
            string totalPath = Path.Combine(targetDir, year.ToString(), month.ToString());
            if (!Directory.Exists(totalPath))
            {
                Directory.CreateDirectory(totalPath);
            }
            totalPath = getName(totalPath, Path.GetFileName(path));
            if (!thumbnail) // for moving image
            {
                File.Move(path, totalPath);
                return totalPath;
            }
            else // for handling thumbnail
            {
                handleThumbnailSize(saveNewImagePath, totalPath);
                return saveNewImagePath;
            }
        }
        #endregion
    }
}
