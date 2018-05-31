using System.IO;
using System.ServiceProcess;
using System.Text;

namespace ImageServerWeb.Models
{
    public class HomePageModel
    {

        public HomePageModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            NumOfImages = 0;
            if (Directory.Exists("C:\\Users\\User\\Desktop\\target\\t"))
            {
                NumOfImages = Directory.GetFiles("C:\\Users\\User\\Desktop\\target\\t", "*", SearchOption.AllDirectories).Length;

            }

            Status = new ServiceController("ImageService").Status == ServiceControllerStatus.Running;
            Etc = ReadInfo();
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

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\User\Documents\Visual Studio 2015\Projects\ImageService\ImageServerWeb\App_Data\Info.txt");

            StringBuilder sb = new StringBuilder();
            sb.Append(lines[0]);
            sb.Append("\n");
            sb.Append(lines[1]);
            Etc = sb.ToString();

            return Etc;

        }

    }
}