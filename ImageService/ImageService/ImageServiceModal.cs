using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;

namespace ImageService
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
            // Determine whether the directory exists.
            if (!Directory.Exists(path))
            {
                if (!CreateDir(m_OutputFolder))
                {
                    result = false;
                    return "Error in creating output folder.";
                }
            }
            DateTime imageDate = GetDateTakenFromImage(path);
            if (!setInDir(imageDate, path))
            {
                result = false;
                return "Error in creating specific folder of year/month";
            }
            result = true;
            return "File in the right directory";

        }

        public bool setInDir(DateTime date, string path)
        {
            int year = date.Year;
            string checkPathY = m_OutputFolder + "/" + year.ToString();
            if (!Directory.Exists(checkPathY))
            {
                if (!CreateDir(checkPathY)) return false;
            }

            int month = date.Month;
            string checkPathM = m_OutputFolder + "/" + year.ToString() + "/" + month.ToString();
            if (!Directory.Exists(checkPathM))
            {
                if (!CreateDir(checkPathM)) return false;
            }
            File.Move(path, checkPathM);
            return true;


        }

        public bool CreateDir(string path)
        {

            try
            {
                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
            finally { }

            return true;

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                System.Drawing.Imaging.PropertyItem propItem = null;
                try
                {
                    propItem =  myImage.GetPropertyItem(36867);
                }
                catch { }
                if (propItem != null)
                {
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
                else
                    return new FileInfo(path).LastWriteTime;
            }
        }

        #endregion

    }

}
