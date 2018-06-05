using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (new ServiceController("ImageService").Status == ServiceControllerStatus.Running)
            {
                Status = "Active";
            } else
            {
                Status = "Not active";
            }
            Students = ReadInfo();
        }

        public void countImages(string path)
        {
            if (Directory.Exists(path))
            {
                NumOfImages = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;

            }
        }

        public List<List<String>> Students
        {
            
            get;
            set;
        }

        [Required]
        [Display(Name = "NumOfImages")]
        public int NumOfImages { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        public List<List<String>> ReadInfo()
        {
            List<List<String>> students = new List<List<String>>{ new List<String> { }, new List<String> { },
            };
            string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Info.txt");
            string[] lines = System.IO.File.ReadAllLines(fileName);

            for(int i = 0; i < lines.Length; i++)
            {
                string[] student = lines[i].Split(' ');

                for (int j = 0; j < student.Length; j++)
                {
                    students[i].Add(student[j]);
                }
            }
            return students;
        }

    }
}