using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ImageServerWeb.Communication;
using ImageServiceCommunication;
using ImageServiceInfrastructure.Enums;
using ImageServiceInfrastructure.Event;

namespace ImageServerWeb.Models
{
    public class ImagesModel
    {
        /// <summary>
        /// c'tor
        /// </summary>
        public ImagesModel()
        {
            // creating an instance of the communication channel
            TcpClient client = TcpClient.Instance;
            // adding the event of the notifications
            client.Channel.MessageRecived += GetMessageFromServer;
            OutputDirectory = null;
            // creating a list of SavedImages
            Images = new List<SavedImages>();
        }

        /// <summary>
        /// This method is being invoken whenever the server sends
        /// the TcpChannel msgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        public void GetMessageFromServer(object sender, DataCommandArgs info)
        {
            var msg = CommandMessage.FromJson(info.Data);
            if (msg == null)
            {
                return;
            }
            if (msg.CommandId == (int)CommandEnum.DeleteCommand) // Delete
            {
                if (msg.Args[0].Equals(SelectedItem.ImagePath) &&
                    msg.Args[1].Equals(SelectedItem.ThumbPath))
                {
                    IsRemoved = true;
                }
            }

        }

        /// <summary>
        /// This method fills the list of images
        /// </summary>
        public void initialize()
        {
            string[] paths = null;
            string name;
            string thumbPath;
            string imagePath;
            DateTime imageDate;
            Images.Clear();
            // search only in the Thumbnails directory
            string dir = OutputDirectory + "\\Thumbnails";
            if (Directory.Exists(dir))
            {
                // get the path of all the images
                paths = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                for (int i = 0; i < paths.Length; i++)
                {
                    // save Thumbnails path
                    thumbPath = paths[i];
                    // get creation time
                    imageDate = File.GetCreationTime(thumbPath);
                    // get file name
                    name = Path.GetFileName(thumbPath);
                    // get full size image path
                    imagePath = Path.Combine(OutputDirectory, 
                        imageDate.Year.ToString(), imageDate.Month.ToString(), name);
                    // create a SavedImages object and add it to the list
                    SavedImages image = new SavedImages(thumbPath, imagePath, name, imageDate);
                    Images.Add(image);
                }
            }
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "SelectedItem")]
        public SavedImages SelectedItem { get; set; }

        /// <summary>
        /// This method changes the images to Base64 in order to
        /// be able to show them, no matter where they are being saved
        /// </summary>
        /// <returns></returns> array of Base64 images
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

        /// <summary>
        /// This method changes only one image to Base64
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns> Base64 image
        public string SingleBase64(string path)
        {
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                string base64 = System.Convert.ToBase64String(bytes);
                return base64;
            }
            return null;
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "OutputDirectory")]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "Images")]
        public List<SavedImages> Images { get; set; }

        /// <summary>
        /// This mehod is being called whenever the user asked
        /// to remove an handler
        /// </summary>
        public void OnRemove(SavedImages image)
        {
            IsRemoved = false;
            string[] args = { image.ImagePath,image.ThumbPath };
            CommandMessage msg = new CommandMessage((int)CommandEnum.DeleteCommand, args);
            TcpClient instance = TcpClient.Instance;
            instance.Channel.Write(msg.ToJson()); // notify the server
            SpinWait.SpinUntil(() => IsRemoved, 4000);// wait until the service updates the data
            if (IsRemoved) // If IsRemoved is true it means than the server deleted the image
            {
                Images.Remove(SelectedItem);
            }
        }

        /// <summary>
        /// Property
        /// </summary>
        [Required]
        [Display(Name = "IsRemoved")]
        public bool IsRemoved { get; set; }

    }
}
