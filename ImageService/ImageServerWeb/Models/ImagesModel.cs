using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        }

        public void initialize()
        {
            string[] paths = null;
            string name;
            string thumbPath;
            string imagePath;
            DateTime imageDate;
            Images.Clear();
            string dir = OutputDirectory + "\\Thumbnails";
            if (Directory.Exists(dir))
            {
                paths = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                for (int i = 0; i < paths.Length; i++)
                {
                    thumbPath = paths[i];

                    imageDate = File.GetCreationTime(thumbPath);
                    name = Path.GetFileName(thumbPath);
                    imagePath = Path.Combine(OutputDirectory, imageDate.Year.ToString(), imageDate.Month.ToString(), name);
                    SavedImages image = new SavedImages(thumbPath, imagePath, name, imageDate);
                    Images.Add(image);
                }
            }
        }



        [Required]
        [Display(Name = "SelectedItem")]
        public SavedImages SelectedItem { get; set; }

        public string[] Base64()
        {
            string[] base64 = new string[Images.Count];
            int i = 0;
            foreach (var image in Images)
            {
                byte[] bytes = File.ReadAllBytes(image.ThumbPath);
                base64[i] = System.Convert.ToBase64String(bytes);
                i++;
            }
            return base64;
        }
        public string SingleBase64(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string base64 = System.Convert.ToBase64String(bytes);
            return base64;
        }

        [Required]
        [Display(Name = "OutputDirectory")]
        public string OutputDirectory { get; set; }

        [Required]
        [Display(Name = "Images")]
        public List<SavedImages> Images { get; set; }
    }
}
