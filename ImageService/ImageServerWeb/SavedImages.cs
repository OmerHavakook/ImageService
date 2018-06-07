using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServerWeb
{
    public class SavedImages
    {
        private Image image;
        private DateTime dateTime;

        public SavedImages(string thumbPath,string imagePath, string name, DateTime date)
        {
            this.ThumbPath = thumbPath;
            this.ImagePath = imagePath;
            this.Name = name;
            this.Date = date;
        }

        public string Name { get; set; }
        public string ThumbPath { get; set; }

        public string ImagePath { get; set; }

        public DateTime Date { get; set; }

    }

    
}
