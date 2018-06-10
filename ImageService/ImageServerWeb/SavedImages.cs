using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServerWeb
{
    /// <summary>
    /// This class saved all of the important data about an image
    /// </summary>
    public class SavedImages
    {
        private Image image;
        private DateTime dateTime;

        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="thumbPath"></param> The path of the Thumbnail
        /// <param name="imagePath"></param> The full size Path name
        /// <param name="name"></param> The image path
        /// <param name="date"></param> The dateTime of the Image
        public SavedImages(string thumbPath,string imagePath, string name, DateTime date)
        {
            this.ThumbPath = thumbPath;
            this.ImagePath = imagePath;
            this.Name = name;
            this.Date = date;
        }

        /// <summary>
        /// Property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        public string ThumbPath { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        public DateTime Date { get; set; }
    } 
}
