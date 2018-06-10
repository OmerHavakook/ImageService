using ImageServerWeb.Models;
using System.Threading;
using System.Web.Mvc;

namespace ImageServerWeb.Controllers
{
    public class HomeController : Controller
    {
        /**
         * Making the model statics in order to create one instance of
         * each them in this class and getting them easily
         */
        static readonly HomePageModel MainModel = new HomePageModel();
        static readonly ConfigModel ConfigModel = new ConfigModel();
        static readonly LogModel LogModel = new LogModel();
        static readonly ImagesModel ImagesModel = new ImagesModel();

        /// <summary>
        /// The main View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConfigModel.Initialize();
            if (MainModel.Initialize()) // check if service status is active
            {
                // wait until data changes
                SpinWait.SpinUntil(() => ConfigModel.OutputDirectory != null, 4000);
                // count num of images
                MainModel.countImages(ConfigModel.OutputDirectory);
            }
            else // if service status is not active
            {
                MainModel.NumOfImages = 0;
            }
            return View(MainModel);
        }

        /// <summary>
        /// Is being called when the user chose to watch the configs
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            // initilie the config model
            ConfigModel.Initialize();
            return View(ConfigModel);
        }

        /// <summary>
        /// Is being called when the user chose to remove a handler from
        /// the list of handlers
        /// </summary>
        /// <param name="id"></param> the index of specific handler in the
        /// list of handlers
        /// <returns></returns>
        public ActionResult RemoveHandler(int id)
        {
            // update the selected item
            ConfigModel.SelectedItem = ConfigModel.Handlers[id];
            return View(ConfigModel);
        }

        /// <summary>
        /// Is being called when the user chose ok on removing a handler
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveHandlerOk()
        {
            ConfigModel.OnRemove();
            // go the config view again
            return RedirectToAction("Config", "Home");
        }

        /// <summary>
        /// Is being called when the user chose to watch the list of logs
        /// </summary>
        /// <returns></returns> View of logs
        public ActionResult Logs()
        {
            LogModel.Initialize();
            return View(LogModel);
        }

        /// <summary>
        /// Is being called when the user chose to watch the thumbnails
        /// </summary>
        /// <returns></returns> the ImageView
        public ActionResult Images()
        {
            // updae output directory
            ImagesModel.OutputDirectory = ConfigModel.OutputDirectory;
            ImagesModel.initialize();
            return View(ImagesModel);
        }

        /// <summary>
        /// If the user chose to remove an imag
        /// </summary>
        /// <param name="id"></param> the index of the image in the list
        /// <returns></returns> View of confirmation
        public ActionResult RemoveImage(int id)
        {
            // update the selected image
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }

        /// <summary>
        /// Is being called when the user chose to view an image
        /// </summary>
        /// <param name="id"></param> the index of the image in the list
        /// <returns></returns> View Image
        public ActionResult ViewImage(int id)
        {
            // update the selected image
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }

        /// <summary>
        /// Is being called whenever the user confirm deleting the image
        /// </summary>
        /// <returns></returns> returns to the Image View
        public ActionResult RemoveImageOk()
        {
            // Deleting images
            ImagesModel.OnRemove(ImagesModel.SelectedItem);
            return RedirectToAction("Images", "Home");
        }
    }
}