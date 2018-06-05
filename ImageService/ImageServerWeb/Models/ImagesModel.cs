using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageServerWeb.Models
{
    public class ImagesModel
    {

        private static Regex r = new Regex(":");


        public ImagesModel()
        {
            OutputDirectory = null;
            Images = new List<SavedImages>();
            /*string path = @"C:\Users\User\Desktop\exitBtn.jpg";
            //Image image = Image.FromFile(path);
            DateTime imageDate = GetDateTakenFromImage(path);

            SavedImages imageS = new SavedImages(path, "Lee", imageDate);
           
            Images.Add(imageS);
            SavedImages imageSO = new SavedImages(path, "Omi", imageDate);

            Images.Add(imageSO);*/

        }

        public void initialize()
        {
            string[] paths = null;
            string name;
            string path;
            DateTime imageDate;
            Images.Clear();
            if (Directory.Exists(OutputDirectory))
            {
                paths = Directory.GetFiles(OutputDirectory, "*", SearchOption.AllDirectories);
                for (int i = 0; i < paths.Length; i++)
                {
                    path = paths[i];
                    imageDate = GetDateTakenFromImage(path);
                    name = Path.GetFileName(path);
                    SavedImages image = new SavedImages(path, name, imageDate);
                    Images.Add(image);
                }
            }  
        }

        public string Base64(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string base64 = System.Convert.ToBase64String(bytes);
            return base64;
        }

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

        [Required]
        [Display(Name = "OutputDirectory")]
        public string OutputDirectory { get; set; }

        [Required]
        [Display(Name = "Images")]
        public List<SavedImages> Images { get; set; }
    }
}
