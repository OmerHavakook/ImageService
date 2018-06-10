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
        /// <summary>
        /// c'tor
        /// </summary>
        public HomePageModel()
        {
            NumOfImages = 0;
            Initialize();
        }

        /// <summary>
        /// Getting the status from the service and reading the information
        /// about the students
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            bool res;
            if (new ServiceController("ImageService").Status ==
                ServiceControllerStatus.Running)
            {
                Status = "Active";
                res = true;
            }
            else
            {
                Status = "Not active";
                res = false;
            }
            Students = ReadInfo();
            return res;
        }

        /// <summary>
        /// This method counts the number of images in theOutputDirectory
        /// </summary>
        /// <param name="path"></param>
        public void countImages(string path)
        {
            if (Directory.Exists(path))
            {
                // update the number of images
                NumOfImages = Directory.GetFiles(path, "*",
                    SearchOption.AllDirectories).Length;
            }
        }

        /// <summary>
        /// Property
        /// </summary>
        public List<List<String>> Students
        {
            get;
            set;
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "NumOfImages")]
        public int NumOfImages { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        /// <summary>
        /// This method read the details about the students
        /// </summary>
        /// <returns></returns>
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