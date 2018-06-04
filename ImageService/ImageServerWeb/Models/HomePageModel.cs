using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Web;

namespace ImageServerWeb.Models
{
    public class HomePageModel
    {

        public HomePageModel()
        {
            NumOfImages = 0;
            Initialize();
        }

        public void Initialize()
        {

            Status = new ServiceController("ImageService").Status == ServiceControllerStatus.Running;
            Etc = ReadInfo();
        }

        public void countImages(string path)
        {
            if (Directory.Exists(path))
            {
                NumOfImages = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;

            }
        }

        public string Etc
        {
            
            get;
            set;
        }
        public int NumOfImages { get; set; }

        public bool Status { get; set; }

        public string ReadInfo()
        {
            string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Info.txt");

            string[] lines = System.IO.File.ReadAllLines(fileName);

            //var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder), "info.txt");
            //System.Console.WriteLine(fileName);
            //string[] lines = System.IO.File.ReadAllLines(@"C:\Users\DELL\Desktop\ImageService\ImageServerWeb\App_Data\Info.txt");

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0]);
            sb.Append("\n");
            sb.Append(lines[1]);
            Etc = sb.ToString();

            return Etc;

        }

    }
}