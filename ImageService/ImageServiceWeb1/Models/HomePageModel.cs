using System;
using System.Text;

namespace ImageServiceWeb1.Models
{
    public class HomePageModel
    {

        public HomePageModel()
        {
            NumOfImages = -1;
            Status = "test";
            Etc = ReadInfo();
            //Etc = "bla";
        }

        public string Etc
        {
            
            get;
            set;
        }
        public int NumOfImages { get; set; }

        public string Status { get; set; }

        public string ReadInfo()
        {

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\User\Documents\Visual Studio 2015\Projects\ImageService\ImageServiceWeb\info.txt");

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0]);
            sb.Append("\n");
            sb.Append(lines[1]);
            Etc = sb.ToString();

            return Etc;

        }

    }
}