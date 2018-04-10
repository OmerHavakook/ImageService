using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using ImageService.Modal;
using System.Threading;

namespace IImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        private static Regex r = new Regex(":");

        public ImageServiceModal(string m_OutputFolder, int m_thumbnailSize)
        {
            this.m_OutputFolder = m_OutputFolder;
            this.m_thumbnailSize = m_thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            string saveNewImagePath = "";
            try
            {
                if (!Directory.Exists(m_OutputFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(m_OutputFolder);
                    di.Attributes = FileAttributes.Hidden;
                }
                DateTime imageDate = GetDateTakenFromImage(path);
                saveNewImagePath = setInDir(imageDate, path, false,null);
                setInDir(imageDate, path, true, saveNewImagePath);

            }
            //catch (FileNotFoundException e)
            //{

            //}
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }
            result = true;
            return "Adding File completed successfully at the path: " + path;
        }

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

        public void handleThumbnailSize(string path, string thumnmailDirPath)
        {

            using (Image thumb = Image.FromFile(path))
            using (Image newIm = thumb.GetThumbnailImage(
              m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero))

            {
                newIm.Save(thumnmailDirPath);
            }

        }

        public string getName(string path,string file)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(file);
            string extension = Path.GetExtension(file);
            //string path = Path.GetDirectoryName(fullPath);
            string newFullPath = Path.Combine(path, Path.GetFileName(file));
            //string newFullPath = Path;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }
            return newFullPath;
        }

        public string setInDir(DateTime date, string path, bool thumbnail, string saveNewImagePath)
        {
            string targetDir = m_OutputFolder;
            // normal copy
            if (thumbnail)
            {
                targetDir = Path.Combine(m_OutputFolder, "Thumbnails");
            }
            int year = date.Year;
            int month = date.Month;
            string totalPath = Path.Combine(targetDir, year.ToString(), month.ToString());
            if (!Directory.Exists(totalPath))
            {
                Directory.CreateDirectory(totalPath);
            }
            totalPath = getName(totalPath, Path.GetFileName(path));
            if (!thumbnail)
            {
                File.Move(path, totalPath);
                return totalPath;
            }
            else
            {
                handleThumbnailSize(saveNewImagePath, totalPath);
                return saveNewImagePath;
            }
        }
        #endregion
    }

}
