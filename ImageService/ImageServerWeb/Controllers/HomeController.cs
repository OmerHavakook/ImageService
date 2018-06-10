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

        // GET: Home
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

        // GET: Config
        public ActionResult Config()
        {
            // initilie the config model
            ConfigModel.Initialize();
            return View(ConfigModel);
        }

        // For removing a handler from the handlers list
        public ActionResult RemoveHandler(int id)
        {
            // update the selected item
            ConfigModel.SelectedItem = ConfigModel.Handlers[id];
            return View(ConfigModel);
        }

        // If user chose ok on removing a handler
        public ActionResult RemoveHandlerOk()
        {
            ConfigModel.OnRemove();
            // go the config view again
            return RedirectToAction("Config", "Home");
        }

        // GET: Logs
        public ActionResult Logs()
        {
            LogModel.Initialize();
            return View(LogModel);
        }

        // GET: Images
        public ActionResult Images()
        {
            // updae output directory
            ImagesModel.OutputDirectory = ConfigModel.OutputDirectory;
            ImagesModel.initialize();
            return View(ImagesModel);
        }

        // If the user chose to remove an image
        public ActionResult RemoveImage(int id)
        {
            // update the selected image
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }

        // If the user chose to view an image
        public ActionResult ViewImage(int id)
        {
            // update the selected image
            ImagesModel.SelectedItem = ImagesModel.Images[id];
            return View(ImagesModel);
        }

        public ActionResult RemoveImageOk()
        {
            System.IO.File.Delete(ImagesModel.SelectedItem.ImagePath);
            System.IO.File.Delete(ImagesModel.SelectedItem.ThumbPath);
            ImagesModel.Images.Remove(ImagesModel.SelectedItem);
            return RedirectToAction("Images", "Home");
        }
    }
}