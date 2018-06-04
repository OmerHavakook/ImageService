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

        public SavedImages(string path, string name, DateTime date)
        {
            this.Path = path;
            this.Name = name;
            this.Date = date;
        }

        public string Name { get; set; }
        public string Path { get; set; }

        public DateTime Date { get; set; }

    }

    
}
